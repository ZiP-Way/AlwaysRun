using Data;
using PlayerSystems;
using SignalsFramework;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

public class Finish : MonoBehaviour
{
    [SerializeField, HideInInspector] private FinishPlatform _finishPlatform = default;
    [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

    #region "Signals"

    public static Subject<Unit> OnPlayerFinished = new Subject<Unit>();
    public static Subject<Vector3> SendFinishPosition = new Subject<Vector3>();

    #endregion

    private void Awake()
    {
        Hub.GameStarted.Subscribe(_ => SendPosition()).AddTo(_ltt);
        Finish.OnPlayerFinished.Delay(TimeSpan.FromSeconds(1f)).Subscribe(_ => DoWIn()).AddTo(_ltt);
    }

    private void SendPosition()
    {
        Finish.SendFinishPosition.Fire(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Finish.OnPlayerFinished.Fire();
            Profile.PlayerProfile.CurrentLevel++;
            _finishPlatform.SetPositionUnderPlayer(player.GetTransform().position);
        }
    }

    private void DoWIn()
    {
        Hub.LevelComplete.Fire();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
        if (_finishPlatform == null) _finishPlatform = GetComponentInChildren<FinishPlatform>();
    }
#endif
}

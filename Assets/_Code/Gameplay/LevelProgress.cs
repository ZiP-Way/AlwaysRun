using PlayerSystems;
using UnityEngine;
using UpdateSys;
using UniRx;
using UniRx.Triggers;
using Utility;
using Data;
using SignalsFramework;

public class LevelProgress : MonoBehaviour, ILateUpdatable
{
    [SerializeField] private Transform _player = default;
    [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

    #region "Signals"

    public static Subject<float> ProgressChanged = new Subject<float>();

    #endregion 

    #region "Fields"

    private Vector3 _finishPosition = default;
    private float _maxDistance = 0;

    #endregion

    private void Awake()
    {
        Finish.SendFinishPosition.Subscribe(position => SetFinishPosition(position)).AddTo(_ltt);
        Hub.LevelFailed.Subscribe(_ => StopChecking()).AddTo(_ltt);
        Hub.LevelComplete.Subscribe(_ => StopChecking()).AddTo(_ltt);
    }

    private void SetFinishPosition(Vector3 position)
    {
        _finishPosition = position;
        _maxDistance = GetDistanceBetweenFinishAndPlayer();
        this.StartLateUpdate();
    }

    private void StopChecking()
    {
        this.StopLateUpdate();
    }

    public void OnSystemLateUpdate(float deltaTime)
    {
        CalculateProgressValue();
    }

    private void CalculateProgressValue()
    {
        float progress = (_maxDistance - GetDistanceBetweenFinishAndPlayer()) / _maxDistance;
        LevelProgress.ProgressChanged.Fire(progress);
    }

    private float GetDistanceBetweenFinishAndPlayer()
    {
        float distance = Vector3.Distance(Vector3.Scale(_player.position, Vector3.forward), new Vector3(0, 0, _finishPosition.z));
        return distance;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
    }
#endif
}

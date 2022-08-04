using Data;
using SignalsFramework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint = default;
    [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

    #region "Signals"

    public static readonly Subject<Vector3> SpawnPlayer = new Subject<Vector3>();

    #endregion

    private void Awake()
    {
        Hub.LevelGenerationCompleted.Subscribe(_ =>
        {
            Level.SpawnPlayer.Fire(_spawnPoint.position);
        }).AddTo(_ltt);
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
    }
#endif
}

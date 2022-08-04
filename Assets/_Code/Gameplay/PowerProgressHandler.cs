using Data;
using DuelSystem;
using ObstacleSystem;
using SignalsFramework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

public class PowerProgressHandler : MonoBehaviour
{
    [SerializeField] private PowerMakerUI _powerMakerUI = default;
    [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

    #region "Signals"

    public static readonly Subject<Unit> OnPowerCharged = new Subject<Unit>();

    #endregion

    #region "Fields"

    private bool _isActive = false;

    private int _targetObstacleCount = 3;
    private int _countPassedObstacles = 0;

    private float _onePassedObstacleValue = 0;

    #endregion

    private void Awake()
    {
        Hub.LoadLevel.Subscribe(_ => ResetValues()).AddTo(_ltt);

        DuelResultController.OnDuelResultChecked.Where(_ => _isActive == true).Subscribe(_ => ResetValues()).AddTo(_ltt);
        Obstacle.OnObstacleTriggeredByPlayer.Subscribe(_ => IncreasePassedObstaclesCount()).AddTo(_ltt);

        _onePassedObstacleValue = 1f / _targetObstacleCount;
    }

    private void ResetValues()
    {
        _isActive = false;
        _countPassedObstacles = 0;

        _powerMakerUI.Disable();
    }

    private void IncreasePassedObstaclesCount()
    {
        if (_isActive) 
            return;

        _countPassedObstacles++;
        _powerMakerUI.Fill(_countPassedObstacles * _onePassedObstacleValue);

        if (_countPassedObstacles == _targetObstacleCount)
        {
            _isActive = true; 

            _powerMakerUI.Enable();
            PowerProgressHandler.OnPowerCharged.Fire();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
    }
#endif
}

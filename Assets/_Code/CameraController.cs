using Cinemachine;
using Data;
using PlayerSystems.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UpdateSys;
using Utility;

public class CameraController : MonoBehaviour, IFixedUpdatable, ILateUpdatable
{
    [SerializeField, HideInInspector] private CinemachineVirtualCamera _virtualCamera = default;
    [SerializeField, HideInInspector] private CinemachineFramingTransposer _composer = default;
    [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

    [Header("Jump Fov Settings")]
    [SerializeField] private float _jumpingFovValue = 40f;
    [SerializeField] private float _speedToJumpFov = 3f;

    [Header("Default Fov Settings")]
    [SerializeField] private float _defaultFovValue = 50f;
    [SerializeField] private float _speedToDefaultFov = 7f;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 5f;

    #region "Signals"

    public static readonly Subject<Unit> DoJumpFov = new Subject<Unit>();
    public static readonly Subject<Unit> DoDefaultFov = new Subject<Unit>();

    #endregion

    #region "Fields"

    private float _savedDeadZoneWidth = 0f;

    private float _targetFov = 0f;
    private float _currentSpeed = 0f;

    #endregion

    private void Awake()
    {
        _savedDeadZoneWidth = _composer.m_DeadZoneWidth;

        Hub.LoadLevel.Subscribe(_ => ResetCameraPosition()).AddTo(_ltt);
        Hub.GameStarted.Subscribe(_ => ReturnToDefaultValues()).AddTo(_ltt);

        OnAirState.OnActiveStateChanged.Subscribe(isActive => CheckWhatToDo(isActive)).AddTo(_ltt);

        Finish.OnPlayerFinished.Subscribe(_ => StartRotate()).AddTo(_ltt);
    }

    private void CheckWhatToDo(bool isJumping)
    {
        if (isJumping)
        {
            SetJumpFov();
        }
        else
        {
            SetDefaultFov();
        }

        this.StartFixedUpdate();
    }

    private void SetDefaultFov()
    {
        _targetFov = _defaultFovValue;
        _currentSpeed = _speedToDefaultFov;
    }

    private void SetJumpFov()
    {
        _targetFov = _jumpingFovValue;
        _currentSpeed = _speedToJumpFov;
    }

    public void OnSystemFixedUpdate(float deltaTime)
    {
        _virtualCamera.m_Lens.FieldOfView = Mathf.MoveTowards(_virtualCamera.m_Lens.FieldOfView, _targetFov, Time.fixedDeltaTime * _currentSpeed);
        if (_virtualCamera.m_Lens.FieldOfView == _targetFov) this.StopFixedUpdate();
    }

    public void OnSystemLateUpdate(float deltaTime)
    {
        Rotate(deltaTime);
    }

    private void StartRotate()
    {
        _composer.m_DeadZoneWidth = 0f;
        this.StartLateUpdate();
    }

    private void Rotate(float deltaTime)
    {
        transform.Rotate(-Vector3.up, _rotationSpeed * deltaTime, Space.World);
    }

    private void ResetCameraPosition()
    {
        this.StopLateUpdate();

        transform.rotation = Quaternion.Euler(12, 0, 0);
        _composer.m_DeadZoneWidth = 0f;
    }

    private void ReturnToDefaultValues()
    {
        _composer.m_DeadZoneWidth = _savedDeadZoneWidth;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_virtualCamera == null) _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (_composer == null) _composer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
    }
#endif
}

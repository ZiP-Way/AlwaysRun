using Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

namespace PlayerSystems.States
{
    public class PlayerStateController : MonoBehaviour
    {
        [Header("States")]
        [SerializeField] private OnRightSideState _onRightSideState = default;
        [SerializeField] private OnLeftSideState _onLeftSideState = default;
        [SerializeField] private OnAirState _onAirState = default;
        [SerializeField] private OnGroundState _onGroundState = default;
        [SerializeField] private ClimbingState _climbingState = default;

        [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

        #region "Fields"

        private PlayerState _currentState = default;

        #endregion

        private void Awake()
        {
            Hub.LoadLevel.Subscribe(_ => DisableCurrentState()).AddTo(_ltt);
            Hub.GameStarted.Subscribe(_ => MoveToOnGroundState()).AddTo(_ltt);
        }

        private void DisableCurrentState()
        {
            if (_currentState != null)
            {
                _currentState.Disable();
                _currentState = null;
            }
        }

        private void SetCurrentState(PlayerState state)
        {
            if (state == null) Debug.LogException(new System.Exception("Attempt to set null state"));

            if (state == _currentState)
            {
                Debug.LogWarning("Trying to set the same state");
                return;
            }

            _currentState = state;
            _currentState.Enable();
        }

        public void MoveToOnGroundState()
        {
            DisableCurrentState();
            SetCurrentState(_onGroundState);
        }

        public void MoveToClimbingState()
        {
            DisableCurrentState();
            SetCurrentState(_climbingState);
        }

        public void MoveToOnLeftSideState()
        {
            DisableCurrentState();
            SetCurrentState(_onLeftSideState);
        }

        public void MoveToOnRightSideState()
        {
            DisableCurrentState();
            SetCurrentState(_onRightSideState);
        }

        public void MoveToOnAirState(float jumpStrength = 12f)
        {
            DisableCurrentState();

            _onAirState.SetParams(jumpStrength);
            SetCurrentState(_onAirState);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
        }
#endif
    }
}

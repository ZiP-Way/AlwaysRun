using Data;
using PlayerSystems.Animations;
using PlayerSystems.Inputs;
using PlayerSystems.MovementSystem.MovementTypes;
using PlayerSystems.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UpdateSys;
using Utility;

namespace PlayerSystems.MovementSystem
{
    public class PlayerMovement : MonoBehaviour, IFixedUpdatable
    {
        [SerializeField] private DefaultMovementType _defaultMovement = default;
        [SerializeField] private ClimbingMovementType _climbingMovement = default;
        [SerializeField] private SideRunningMovementType _sideRunningMovement = default;

        [Header("Other Components")]
        [SerializeField] private Transform _body = default;

        [SerializeField, HideInInspector] private CharacterController _characterController = default;
        [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

        #region "Fields"

        private MovementType _currentMovement = default;

        private bool _isGrounded = false;
        private bool _isXValueRepayment = false;
        private bool _isOnlyYMovement = false;

        private float _currentXDirection = 0f;

        #endregion

        private void Awake()
        {
            ClimbingState.IsClimbingUpStarted.Subscribe(_ => StopMovement()).AddTo(_ltt);
            ClimbingState.IsClimbingUpDone.Subscribe(_ => StartMovement()).AddTo(_ltt);

            PlayerInputDetection.SwipingStarted.Subscribe(_ => StopXValueRepayment()).AddTo(_ltt);
            PlayerInputDetection.SwipeValueChanged.Subscribe(swipeDirection => ChangeXDirection(swipeDirection)).AddTo(_ltt);
            PlayerInputDetection.SwipingStopped.Subscribe(_ => StartXValueRepayment()).AddTo(_ltt);

            Hub.GameStarted.Subscribe(_ => StartMovement()).AddTo(_ltt);

            Hub.LoadLevel.Subscribe(_ => ResetValues()).AddTo(_ltt);
            Hub.LevelFailed.Subscribe(_ => StopMovement()).AddTo(_ltt);
            Hub.LevelComplete.Subscribe(_ => StopMovement()).AddTo(_ltt);

            Finish.OnPlayerFinished.Subscribe(_ => _isOnlyYMovement = true).AddTo(_ltt);
        }

        private void OnDisable()
        {
            StopMovement();
        }

        public void OnSystemFixedUpdate(float deltaTime)
        {
            if (_isXValueRepayment) DoXValueRepayment(deltaTime);
            Move(deltaTime);
            Rotate(deltaTime);
        }

        public void Jump(float jumpStrenght)
        {
            _currentMovement.DoJump(jumpStrenght);
        }

        public void SetDefaultMovement(bool isGrounded)
        {
            _isGrounded = isGrounded;

            _currentMovement = _defaultMovement;
            _currentMovement.ResetValues();
        }

        public void SetClimbingMovement()
        {
            _currentMovement = _climbingMovement;
            _currentMovement.ResetValues();
        }

        public void SetSideRunningMovement()
        {
            _currentMovement = _sideRunningMovement;
            _currentMovement.ResetValues();
        }

        private void StartMovement()
        {
            this.StartFixedUpdate();
        }

        private void StopMovement()
        {
            this.StopFixedUpdate();
        }

        private void StartXValueRepayment()
        {
            _isXValueRepayment = true;
        }

        private void StopXValueRepayment()
        {
            _isXValueRepayment = false;
        }

        private void DoXValueRepayment(float deltaTime)
        {
            _currentXDirection = Mathf.MoveTowards(_currentXDirection, 0, deltaTime * 4);
        }

        private void ResetValues()
        {
            _isOnlyYMovement = false;
            _body.rotation = Quaternion.identity;
            StopMovement();
        }

        private void Move(float deltaTime)
        {
            if (_isOnlyYMovement)
            {
                Vector3 direction = Vector3.Scale(_currentMovement.GetMovementDirection(_currentXDirection, _isGrounded), Vector3.up);
                _characterController.Move(direction * deltaTime);
            }
            else
            {
                _characterController.Move(_currentMovement.GetMovementDirection(_currentXDirection, _isGrounded) * deltaTime);
            }
        }

        private void Rotate(float deltaTime)
        {
            _body.rotation = Quaternion.Lerp(_body.rotation, _currentMovement.GetAngle(_currentXDirection, deltaTime, out float rotationSpeed), rotationSpeed);
        }

        private void ChangeXDirection(Vector3 inputedDirection)
        {
            _currentXDirection = inputedDirection.x;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
            if (_characterController == null) _characterController = GetComponent<CharacterController>();
        }
#endif
    }
}

using PlayerSystems.MovementSystem;
using SignalsFramework;
using UniRx;
using UnityEngine;

namespace PlayerSystems.States
{
    public class ClimbingState : PlayerState
    {
        [SerializeField] private RayObstacleDetection _forwardObstacleDetection = default;

        [SerializeField] private RayObstacleDetection _rightObstacleDetectin = default;
        [SerializeField] private RayObstacleDetection _leftObstacleDetection = default;

        [SerializeField] private RayObstacleDetection _groundDetection = default;

        [SerializeField] private PlayerMovement _playerMovement = default;

        #region "Signals"

        public static readonly Subject<Unit> IsClimbingUpStarted = new Subject<Unit>();
        public static readonly Subject<Unit> IsClimbingUpDone = new Subject<Unit>();

        #endregion

        #region "Fields"

        private bool _isClimbingUp = false;

        #endregion

        private void Awake()
        {
            Init();
        }

        public override void Init()
        {
            _forwardObstacleDetection.IsDetected.Where(state => state == false && IsActive).Subscribe(_ => ClimbingUp()).AddTo(Ltt);
            _rightObstacleDetectin.IsDetected.Where(state => state == false && IsActive).Subscribe(_ => StateController.MoveToOnAirState()).AddTo(Ltt);
            _leftObstacleDetection.IsDetected.Where(state => state == false && IsActive).Subscribe(_ => StateController.MoveToOnAirState()).AddTo(Ltt);

            ClimbingState.IsClimbingUpDone.Where(_ => _isClimbingUp == true).Subscribe(_ => CheckGroundDetectoion()).AddTo(Ltt);

            Disable();
        }
        public override void Enable()
        {
            base.Enable();
            _playerMovement.SetClimbingMovement();

            if (_forwardObstacleDetection.Check())
            {
                PlayerAnimator.SetBool("IsClimbing", true);
            }
            else
            {
                ClimbingUp();
            }
        }

        public override void Disable()
        {
            base.Disable();
            _isClimbingUp = false;

            PlayerAnimator.ResetTrigger("DoClimbingUp");
            PlayerAnimator.SetBool("IsClimbing", false);

            ClimbingState.IsClimbingUpDone.Fire();
        }

        protected override void EnableDetections()
        {
            _leftObstacleDetection.gameObject.SetActive(true);
            _rightObstacleDetectin.gameObject.SetActive(true);
            _groundDetection.gameObject.SetActive(true);
            _forwardObstacleDetection.gameObject.SetActive(true);
        }

        protected override void DisableDetections()
        {
            _leftObstacleDetection.gameObject.SetActive(false);
            _rightObstacleDetectin.gameObject.SetActive(false);
            _groundDetection.gameObject.SetActive(false);
            _forwardObstacleDetection.gameObject.SetActive(false);
        }

        private void ClimbingUp()
        {
            if (_isClimbingUp) return;
            _isClimbingUp = true;

            ClimbingState.IsClimbingUpStarted.Fire();
            PlayerAnimator.SetTrigger("DoClimbingUp");
        }

        private void CheckGroundDetectoion()
        {
            if (_groundDetection.Check())
            {
                StateController.MoveToOnGroundState();
            }
            else
            {
                StateController.MoveToOnAirState();
            }

            _isClimbingUp = false;
        }
    }
}

using PlayerSystems.MovementSystem;
using UniRx;
using UnityEngine;

namespace PlayerSystems.States
{
    public class OnRightSideState : PlayerState
    {
        [SerializeField] private RayObstacleDetection _rightSideDetection = default;
        [SerializeField] private RayObstacleDetection _forwardSideDetection = default;

        [SerializeField] private PlayerMovement _playerMovement = default;

        private void Awake()
        {
            Init();
        }

        public override void Init()
        {
            _rightSideDetection.IsDetected.Where(state => state == false && IsActive).Subscribe(_ => StateController.MoveToOnAirState()).AddTo(Ltt);
            _forwardSideDetection.IsDetected.Where(state => state == true && IsActive).Subscribe(_ => StateController.MoveToClimbingState()).AddTo(Ltt);

            Disable();
        }

        public override void Enable()
        {
            base.Enable();

            PlayerAnimator.SetBool("IsRightSide", true);
            _playerMovement.SetSideRunningMovement();
        }

        public override void Disable()
        {
            base.Disable();

            PlayerAnimator.SetBool("IsRightSide", false);
        }

        protected override void EnableDetections()
        {
            _rightSideDetection.gameObject.SetActive(true);
            _forwardSideDetection.gameObject.SetActive(true);
        }

        protected override void DisableDetections()
        {
            _rightSideDetection.gameObject.SetActive(false);
            _forwardSideDetection.gameObject.SetActive(false);
        }
    }
}

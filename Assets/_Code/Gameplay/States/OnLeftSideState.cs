using PlayerSystems.MovementSystem;
using UniRx;
using UnityEngine;

namespace PlayerSystems.States
{
    public class OnLeftSideState : PlayerState
    {
        [SerializeField] private RayObstacleDetection _leftSideDetection = default;
        [SerializeField] private RayObstacleDetection _forwardSideDetection = default;

        [SerializeField] private PlayerMovement _playerMovement = default;

        private void Awake()
        {
            Init();
        }

        public override void Init()
        {
            _leftSideDetection.IsDetected.Where(state => state == false && IsActive).Subscribe(_ => StateController.MoveToOnAirState()).AddTo(Ltt);
            _forwardSideDetection.IsDetected.Where(state => state == true && IsActive).Subscribe(_ => StateController.MoveToClimbingState()).AddTo(Ltt);

            Disable();
        }

        public override void Enable()
        {
            base.Enable();

            PlayerAnimator.SetBool("IsLeftSide", true);
            _playerMovement.SetSideRunningMovement();
        }

        public override void Disable()
        {
            base.Disable();

            PlayerAnimator.SetBool("IsLeftSide", false);
        }

        protected override void EnableDetections()
        {
            _leftSideDetection.gameObject.SetActive(true);
            _forwardSideDetection.gameObject.SetActive(true);
        }

        protected override void DisableDetections()
        {
            _leftSideDetection.gameObject.SetActive(false);
            _forwardSideDetection.gameObject.SetActive(false);
        }
    }
}

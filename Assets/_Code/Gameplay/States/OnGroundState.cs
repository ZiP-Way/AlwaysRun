using PlayerSystems.MovementSystem;
using UniRx;
using UnityEngine;

namespace PlayerSystems.States
{
    public class OnGroundState : PlayerState
    {
        [SerializeField] private RayObstacleDetection _forwardObstacleDetection = default;
        [SerializeField] private GroundDetection _groundDetection = default;

        [SerializeField] private PlayerMovement _playerMovement = default;

        private void Awake()
        {
            Init();
        }

        public override void Init()
        {
            _groundDetection.IsOnGround.Where(state => state == false && IsActive).Subscribe(_ => StateController.MoveToOnAirState()).AddTo(Ltt);
            _forwardObstacleDetection.IsDetected.Where(state => state == true && IsActive).Subscribe(_ => StateController.MoveToClimbingState()).AddTo(Ltt);
        }

        public override void Enable()
        {
            base.Enable();

            _playerMovement.SetDefaultMovement(true);
            PlayerAnimator.SetBool("IsRunning", true);
        }

        public override void Disable()
        {
            base.Disable();

            PlayerAnimator.SetBool("IsRunning", false);
        }

        protected override void EnableDetections()
        {
            _forwardObstacleDetection.gameObject.SetActive(true);
            _groundDetection.gameObject.SetActive(true);
        }

        protected override void DisableDetections()
        {
            _forwardObstacleDetection.gameObject.SetActive(false);
            _groundDetection.gameObject.SetActive(false);
        }
    }
}

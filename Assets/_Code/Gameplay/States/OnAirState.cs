using PlayerSystems.MovementSystem;
using SignalsFramework;
using System.Collections;
using UniRx;
using UnityEngine;

namespace PlayerSystems.States
{
    public class OnAirState : PlayerState
    {
        [SerializeField] private RayObstacleDetection _forwardObstacleDetection = default;
        [SerializeField] private GroundDetection _groundDetection = default;

        [SerializeField] private RayObstacleDetection _rightSideDetection = default;
        [SerializeField] private RayObstacleDetection _leftSideDetection = default;

        [SerializeField] private PlayerMovement _playerMovement = default;

        #region "Signals"

        public static readonly Subject<bool> OnActiveStateChanged = new Subject<bool>();

        #endregion

        #region "Fields"

        private Coroutine _coroutine;
        private float _jumpStrength = 0f;

        #endregion

        private void Awake()
        {
            Init();
        }

        public override void Init()
        {
            _groundDetection.IsOnGround.Where(state => state == true && IsActive).Subscribe(_ => StateController.MoveToOnGroundState()).AddTo(Ltt);
            _forwardObstacleDetection.IsDetected.Where(state => state == true && IsActive).Subscribe(_ => StateController.MoveToClimbingState()).AddTo(Ltt);
            _rightSideDetection.IsDetected.Where(state => state == true && IsActive).Subscribe(_ => StateController.MoveToOnRightSideState()).AddTo(Ltt);
            _leftSideDetection.IsDetected.Where(state => state == true && IsActive).Subscribe(_ => StateController.MoveToOnLeftSideState()).AddTo(Ltt);

            Disable();
        }

        public override void Enable()
        {
            base.Enable();

            PlayerAnimator.SetTrigger("DoJump");

            _playerMovement.SetDefaultMovement(false);
            _playerMovement.Jump(_jumpStrength);

            OnAirState.OnActiveStateChanged.Fire(true);
        }

        public override void Disable()
        {
            base.Disable();

            OnAirState.OnActiveStateChanged.Fire(false);
        }

        public void SetParams(float jumpStrength)
        {
            _jumpStrength = jumpStrength;
        }

        protected override void EnableDetections()
        {
            _forwardObstacleDetection.gameObject.SetActive(true);
            _leftSideDetection.gameObject.SetActive(true);
            _rightSideDetection.gameObject.SetActive(true);

            if (_coroutine == null)
                _coroutine = StartCoroutine(DelayTest());
        }

        protected override void DisableDetections()
        {
            _forwardObstacleDetection.gameObject.SetActive(false);
            _leftSideDetection.gameObject.SetActive(false);
            _rightSideDetection.gameObject.SetActive(false);
            _groundDetection.gameObject.SetActive(false);

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator DelayTest()
        {
            yield return new WaitForSeconds(0.5f);
            _groundDetection.gameObject.SetActive(true);

            _coroutine = null;
        }
    }
}
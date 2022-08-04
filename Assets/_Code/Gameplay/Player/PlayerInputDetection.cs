using SignalsFramework;
using UniRx;
using UnityEngine;
using UnityEngine.Profiling;
using UpdateSys;

namespace PlayerSystems.Inputs
{
    public class PlayerInputDetection : MonoBehaviour, IUpdatable
    {
        #region "Signals"

        public static readonly Subject<Unit> SwipingStarted = new Subject<Unit>();
        public static readonly Subject<Vector2> SwipeValueChanged = new Subject<Vector2>();
        public static readonly Subject<Unit> SwipingStopped = new Subject<Unit>();

        #endregion

        #region "Fields"

        private Vector2 _startTapPosition = default,
                _currentTapPosition = default;

        private float _maxSwipeValue = 0f;
        private Vector2 _currentSwipeValue = Vector2.zero;

        #endregion

        private void Awake()
        {
            _maxSwipeValue = Screen.width * 0.3f; // 30% percent of the screen size
        }

        private void OnEnable()
        {
            this.StartUpdate();
        }

        private void OnDisable()
        {
            this.StopUpdate();
        }

        public void OnSystemUpdate(float deltaTime)
        {
            Profiler.BeginSample("Swiping");

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _startTapPosition = Input.mousePosition;
                PlayerInputDetection.SwipingStarted.Fire();
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (_startTapPosition == Vector2.zero) _startTapPosition = Input.mousePosition;

                _currentTapPosition = Input.mousePosition;

                _currentSwipeValue.x = (_currentTapPosition.x - _startTapPosition.x) / _maxSwipeValue;
                _currentSwipeValue.x = Mathf.Clamp(_currentSwipeValue.x, -1.0f, 1.0f);

                _currentSwipeValue.y = (_currentTapPosition.y - _startTapPosition.y) / _maxSwipeValue;
                _currentSwipeValue.y = Mathf.Clamp(_currentSwipeValue.y, -1.0f, 1.0f);

                PlayerInputDetection.SwipeValueChanged.Fire(_currentSwipeValue);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                PlayerInputDetection.SwipingStopped.Fire();
            }

            Profiler.EndSample();
        }

        public void ResetStartTapPosition()
        {
            _startTapPosition = Input.mousePosition;
        }
    }
}

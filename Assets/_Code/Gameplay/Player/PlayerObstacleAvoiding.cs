using PlayerSystems.Animations;
using PlayerSystems.MovementSystem;
using PlayerSystems.States;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystems.ObstacleAvoiding
{
    public class PlayerObstacleAvoiding : MonoBehaviour
    {
        [SerializeField] private Animator _animator = default;

        [SerializeField, HideInInspector] private PlayerMovement _playerMovement = default;
        [SerializeField, HideInInspector] private PlayerStateController _stateController = default;

        #region "Fields"

        private PlayerAnimatorHashCodes _animatorHashCodes = new PlayerAnimatorHashCodes();
        private List<int> _avoidingLowObstaclesAnimationHashCodes = new List<int>();
        protected List<int> _avoidingHybridObstaclesAnimationHashCodes = new List<int>();

        #endregion

        private void Awake()
        {
            _avoidingLowObstaclesAnimationHashCodes.Add(_animatorHashCodes.DoFlipTrigger);
            _avoidingLowObstaclesAnimationHashCodes.Add(_animatorHashCodes.DoTwistFlipTrigger);
            _avoidingLowObstaclesAnimationHashCodes.Add(_animatorHashCodes.DoFlipTrickTrigger);
            _avoidingLowObstaclesAnimationHashCodes.Add(_animatorHashCodes.DoJumping1Trigger);
            _avoidingLowObstaclesAnimationHashCodes.Add(_animatorHashCodes.DoJumping2Trigger);

            _avoidingHybridObstaclesAnimationHashCodes.Add(_animatorHashCodes.DoJumpOver);
            _avoidingHybridObstaclesAnimationHashCodes.Add(_animatorHashCodes.DoSlide);
            _avoidingHybridObstaclesAnimationHashCodes.Add(_animatorHashCodes.DoUnder);
        }

        public void AvoidLowObstacle()
        {
            _animator.SetTrigger(_avoidingLowObstaclesAnimationHashCodes[Random.Range(0, _avoidingLowObstaclesAnimationHashCodes.Count)]);
        }

        public void AvoidHybridObstacle()
        {
            _animator.SetTrigger(_avoidingHybridObstaclesAnimationHashCodes[Random.Range(0, _avoidingHybridObstaclesAnimationHashCodes.Count)]);
        }

        public void JumpOnSpringBoard()
        {
            _stateController.MoveToOnAirState(20f);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_playerMovement == null) _playerMovement = GetComponent<PlayerMovement>();
            if(_stateController == null) _stateController = GetComponent<PlayerStateController>();
        }
#endif
    }
}
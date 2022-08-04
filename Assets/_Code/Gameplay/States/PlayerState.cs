using UniRx.Triggers;
using UnityEngine;
using Utility;

namespace PlayerSystems.States
{
    public abstract class PlayerState : MonoBehaviour
    {
        [SerializeField] private Animator _animator = default;
        [SerializeField, HideInInspector] private PlayerStateController _playerStateController = default;
        [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

        #region "Properties"

        protected Animator PlayerAnimator => _animator;

        #endregion

        #region "Properties"

        protected ObservableDestroyTrigger Ltt => _ltt;

        protected bool IsActive => _isActive;
        protected PlayerStateController StateController => _playerStateController;

        #endregion

        #region "Fields"

        private bool _isActive = false;

        #endregion

        public abstract void Init();

        protected abstract void EnableDetections();

        protected abstract void DisableDetections();

        public virtual void Enable()
        {
            _isActive = true;
            EnableDetections();
        }

        public virtual void Disable()
        {
            _isActive = false;
            DisableDetections();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
            if (_playerStateController == null) _playerStateController = GetComponentInParent<PlayerStateController>();
        }
#endif
    }
}

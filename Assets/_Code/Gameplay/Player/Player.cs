using Data;
using DuelSystem;
using SignalsFramework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

namespace PlayerSystems
{
    public class Player : MonoBehaviour, IDuelMember
    {
        [SerializeField] private Animator _animator = default;
        [SerializeField] private ParticleSystem[] _shootVFX = default;

        [SerializeField, HideInInspector] private AimTarget _aimTarget = default;
        [SerializeField, HideInInspector] private CharacterController _characterController = default;
        [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

        private void Awake()
        {
            Hub.GameStarted.Subscribe(_ => EnableChecking()).AddTo(_ltt);
            Hub.LoadLevel.Subscribe(_ => ResetState()).AddTo(_ltt);
            Finish.OnPlayerFinished.Subscribe(_ => _animator.SetTrigger("DoVictoryDance")).AddTo(_ltt);
        }

        public void Die()
        {
            _animator.SetTrigger("DoDeath");
            Hub.LevelFailed.Fire();
        }

        public void DoShoot(IDuelMember target)
        {
            TakeAim(target.GetTransform());
            foreach (var vfx in _shootVFX)
            {
                vfx.Play();
            }
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void TakeAim(Transform target)
        {
            _aimTarget.SetTarget(target);
        }

        private void EnableChecking()
        {
            _characterController.detectCollisions = true;
        }

        private void DisableChecking()
        {
            _characterController.detectCollisions = false;
        }

        private void ResetState()
        {
            _animator.SetTrigger("DoIdle");
            DisableChecking();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_aimTarget == null) _aimTarget = GetComponent<AimTarget>();
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
            if(_characterController == null) _characterController = GetComponent<CharacterController>();
        }
#endif
    }
}
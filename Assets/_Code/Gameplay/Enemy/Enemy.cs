using DuelSystem;
using UnityEngine;
using UpdateSys;

namespace EnemySystems
{
    public class Enemy : MonoBehaviour, IDuelMember, ILateUpdatable
    {
        [SerializeField] private Animator _animator = default;
        [SerializeField] private Transform _body = default;

        [SerializeField] private ParticleSystem _particleSystem = default;

        #region "Fields"

        private Transform _target;

        #endregion

        private void OnDisable()
        {
            this.StopLateUpdate();
        }

        public void Die()
        {
            _animator.SetTrigger("Die");
        }

        public void DoShoot(IDuelMember target)
        {
            _target = target.GetTransform();
            _particleSystem.Play();

            _animator.SetTrigger("DoShoot");

            this.StartLateUpdate();
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void OnSystemLateUpdate(float deltaTime)
        {
            Vector3 targetDirection = _target.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(_body.forward, targetDirection, deltaTime * 5f, 0.0f);
            _body.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}

using EnemySystems;
using SignalsFramework;
using UniRx;
using UnityEngine;

namespace DuelSystem
{
    public class DuelArea : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy = default;

        #region "Signals/Events"

        public static readonly Subject<Unit> OnPlayerTriggerEnter = new Subject<Unit>();

        #endregion

        #region "Fields"

        private IDuelMember _enemyMember = default;
        private IDuelMember _playerMember = default;

        private CompositeDisposable _disposables = default;

        #endregion

        private void Awake()
        {
            _enemyMember = _enemy;
            _disposables = new CompositeDisposable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDuelMember player))
            {
                _playerMember = player;

                DuelResultController.OnDuelResultChecked.Subscribe(duelResult => GettingDuelResult(duelResult)).AddTo(_disposables);
                DuelArea.OnPlayerTriggerEnter.Fire();
            }
        }

        private void GettingDuelResult(DuelResultType duelResult)
        {
            _disposables.Clear();

            if (duelResult == DuelResultType.Win)
            {
                _playerMember.DoShoot(_enemyMember);
                _enemyMember.Die();
            }
            else if (duelResult == DuelResultType.Lose)
            {
                _enemyMember.DoShoot(_playerMember);
                _playerMember.Die();
            }
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
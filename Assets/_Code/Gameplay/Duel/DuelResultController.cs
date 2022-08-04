using Data;
using SignalsFramework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

namespace DuelSystem
{
    public class DuelResultController : MonoBehaviour
    {
        [SerializeField] private CollectedBulletsCounter _collectedBulletsCounter = default;
        [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

        #region "Signals"

        public static readonly Subject<DuelResultType> OnDuelResultChecked = new Subject<DuelResultType>();

        #endregion

        #region "Fields"

        private DuelResultType _duelResult = default;
        private bool _isPlayerHasPriority = false;

        #endregion

        private void Awake()
        {
            Hub.LoadLevel.Subscribe(_ => _isPlayerHasPriority = false).AddTo(_ltt);
            DuelArea.OnPlayerTriggerEnter.Subscribe(_ => CheckDuelResult(_isPlayerHasPriority)).AddTo(_ltt);
            PowerProgressHandler.OnPowerCharged.Subscribe(_ => _isPlayerHasPriority = true).AddTo(_ltt);
        }

        private void CheckDuelResult(bool isPlayerHasPriority)
        {
            if (_collectedBulletsCounter.CollectedBulletsCount <= 0 && !isPlayerHasPriority)
            {
                _duelResult = DuelResultType.Lose;
            }
            else
            {
                _isPlayerHasPriority = false;
                _duelResult = DuelResultType.Win;
            }

            OnDuelResultChecked.Fire(_duelResult);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
        }
#endif
    }
}
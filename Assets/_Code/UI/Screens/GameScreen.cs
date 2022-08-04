using Data;
using EditorExtensions.Attributes;
using Profile;
using Tweening;
using UI.Progress;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

namespace UI.Screens
{
    /// <summary>
    /// Game UI screen logic
    /// </summary>
    public class GameScreen : MonoBehaviour
    {
        [SerializeField, RequireInput]
        private GameObject _root = default;

        [SerializeField, RequireInput]
        private ABTweener _fadeTweener = default;

        [SerializeField, RequireInput]
        private SoftCurrencyPanel _softCurrencyPanel = default;
        
        [SerializeField]
        private BulletsCounterUI _bulletsCounterUI = default;

        [SerializeField, HideInInspector]
        private ObservableDestroyTrigger _ltt = default;

        [SerializeField] private LevelProgressBarSingle _progressBar = default;

        #region [Fields]

        private bool _isActive;

        #endregion

        private void Awake()
        {
            _bulletsCounterUI.Init();

            _root.SetActive(false);
            _progressBar.Init();

            Hub.GameStarted.Subscribe(_ =>
            {
                ResetToInitialState();
                Toggle(true);
            }).AddTo(_ltt);

            Hub.LevelComplete.Subscribe(_ => Toggle(false)).AddTo(_ltt);
            Hub.LevelFailed.Subscribe(_ => Toggle(false)).AddTo(_ltt);

            Hub.RequestLobbyTransition.Subscribe(_ =>
            {
                if (_isActive) Toggle(false);
            }).AddTo(_ltt);
        }

        private void Start()
        {
            Toggle(false);
        }

        private void ResetToInitialState()
        {
            _softCurrencyPanel.SetValue(PlayerProfile.SoftCurrency);

            // Can be extended
        }

        private void Toggle(bool state)
        {
            _isActive = state;

            if (state)
            {
                _fadeTweener.DoB();
            }
            else
            {
                _fadeTweener.DoA();
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
            if (_softCurrencyPanel == null) _softCurrencyPanel = GetComponentInChildren<SoftCurrencyPanel>(true);
        }
#endif
    }
}

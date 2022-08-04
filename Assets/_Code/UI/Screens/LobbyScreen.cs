using Data;
using Data.Levels;
using EditorExtensions.Attributes;
using Profile;
using SignalsFramework;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Screens
{
    /// <summary>
    /// Lobby Screen display logic
    /// </summary>
    public class LobbyScreen : MonoBehaviour
    {
        [SerializeField, RequireInput]
        private GameObject _root = default;

        [SerializeField, RequireInput]
        private Button _playBtn = default;

        [SerializeField, RequireInput]
        private SoftCurrencyPanel _softCurrencyPanel = default;

        [SerializeField, RequireInput]
        private GameObject _overlayClickBlocker = default;

        [Space]
        [SerializeField, RequireInput]
        private TMP_Text _levelText = default;

        [SerializeField]
        private string _levelStr = "Level {0}";

        [SerializeField, HideInInspector]
        private ObservableDestroyTrigger _ltt = default;

        #region [Fields]

        private bool _startGameRequested;
        private bool _isActive;

        #endregion

        private void Awake()
        {
            _playBtn.onClick.AddListener(OnPlayBtnClicked);

            Hub.LevelGenerationCompleted.Subscribe(metaData =>
            {
                ResetToInitialState(metaData);
                Toggle(true);
            }).AddTo(_ltt);

            Hub.GameStarted.Subscribe(_ =>
            {
                _overlayClickBlocker.SetActive(true);
                _startGameRequested = true;

                Toggle(false);
            }).AddTo(_ltt);

            _overlayClickBlocker.SetActive(false);
            Toggle(true);
        }

        private void ResetToInitialState(LevelMetaData metaData)
        {
            _startGameRequested = false;
            _overlayClickBlocker.SetActive(false);
            _levelText.text = string.Format(_levelStr, (metaData.VisualLevelIndex + 1).CachedString());
        }

        private void OnPlayBtnClicked()
        {
            if (_startGameRequested) return;
            if (!_isActive) return;

            Hub.GameStarted.Fire();
        }

        private void Toggle(bool state)
        {
            if (_isActive == state) return;

            _root.SetActive(state);
            _isActive = state;

            if (state)
                _softCurrencyPanel.SetValue(PlayerProfile.SoftCurrency);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
        }
#endif
    }
}
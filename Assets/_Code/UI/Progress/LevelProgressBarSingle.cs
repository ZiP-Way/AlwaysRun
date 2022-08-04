using Data;
using TMPro;
using UI.ProgressBar;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

namespace UI.Progress
{
    /// <summary>
    /// Display logic for single segment progress bar
    /// </summary>
    public class LevelProgressBarSingle : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarUI _progressBar = default;

        [SerializeField]
        private TMP_Text _currentLevelText = default;

        [SerializeField, HideInInspector]
        private ObservableDestroyTrigger _ltt = default;

        public void Init()
        {
            Hub.LoadLevel.Subscribe(_ => _progressBar.ProgressChanged(0f, true)).AddTo(_ltt);
            LevelProgress.ProgressChanged.Subscribe(progress => _progressBar.ProgressChanged(progress)).AddTo(_ltt);

            Hub.LevelGenerationCompleted.Subscribe(x => UpdateLevelText(x.VisualLevelIndex)).AddTo(_ltt);
        }

        private void UpdateLevelText(int currentLevel)
        {
            _currentLevelText.text = (currentLevel + 1).CachedString();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
        }
#endif
    }
}
using Data;
using EditorExtensions.Attributes;
using SignalsFramework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Utility {
    /// <summary>
    /// Example / utility for incrementing level progress in game
    /// </summary>
    public class IncrementLevelProgressBtn : MonoBehaviour {
        [SerializeField, RequireInput]
        private Button _btn = default;

        [SerializeField, HideInInspector]
        private ObservableDestroyTrigger _ltt = default;

        #region [Fields]

        private float _currentProgress;
        
        #endregion

        private void Awake() {
            Hub.GenerateLevel.Subscribe(_ => _currentProgress = 0).AddTo(_ltt);
            
            _btn.onClick.AddListener(IncrementLevelProgress);
        }

        private void IncrementLevelProgress() {
            if (_currentProgress >= 1f) return;
            
            _currentProgress += 0.05f;
            _currentProgress = _currentProgress > 1f ? 1f : _currentProgress;

            Hub.LevelProgressChanged.Fire(_currentProgress);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
            if (_btn == null) TryGetComponent(out _btn);
        }
#endif
    }
}

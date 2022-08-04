using Data;
using Data.Levels;
using EditorExtensions.Attributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

namespace UI.Progress {
   /// <summary>
   /// Progress bar for current level (segmented)
   /// </summary>
   public class LevelProgressBarSegmented : MonoBehaviour {
      [SerializeField, RequireInput]
      private ProgressBarMultiSegmentUI _progressBar = default;

      [SerializeField, HideInInspector]
      private ObservableDestroyTrigger _ltt = default;
      
      #region [Fields]

      private int _totalStages;
      
      #endregion

      private void Awake() {
         Hub.LevelProgressChanged.Subscribe(UpdateProgress).AddTo(_ltt);
         Hub.LevelGenerationCompleted.Subscribe(SetupProgressBar).AddTo(_ltt);
      }

      private void SetupProgressBar(LevelMetaData data) {
         _totalStages = data.LevelData.StagesPerLevel;
         
         _progressBar.SetSegmentCount(_totalStages);
         _progressBar.FillFirst(0, true, true);
      }

      private void UpdateProgress(float progress) {
         int filledSegments = Mathf.FloorToInt(_totalStages * progress);
         _progressBar.FillFirst(filledSegments);
      }

#if UNITY_EDITOR
      protected virtual void OnValidate() {
         if (_progressBar == null) _progressBar = GetComponentInChildren<ProgressBarMultiSegmentUI>(true);
         if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
      }
#endif
   }
}
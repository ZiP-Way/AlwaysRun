using Data;
using Data.Levels;
using EditorExtensions.Attributes;
using Profile;
using SignalsFramework;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Levels
{
    /// <summary>
    /// Example how to use signals for level generation.
    /// Can be removed or extended at will.
    /// </summary>
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField, RequireInput] private LevelList _levelList = default;
        [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

        #region "Fields"

        private int _currentLevel = -1;

        #endregion

        private void Awake()
        {
            Hub.LoadLevel.Subscribe(_ => UnLoadLevel()).AddTo(_ltt);
        }

        private void Start()
        {
            Hub.LoadLevel.Fire();
        }

        private void UnLoadLevel()
        {
            if (_currentLevel >= 0)
            {
                AsyncOperation levelUnload = SceneManager.UnloadSceneAsync("Level" + _currentLevel); // current level
                Resources.UnloadUnusedAssets();
                levelUnload.completed += x => GenerateLevel();
            }
            else
            {
                GenerateLevel();
            }
        }

        private void GenerateLevel()
        {
            int visualLevelIndex = PlayerProfile.CurrentLevel;
            int level = visualLevelIndex;

            List<LevelData> allLevels = _levelList.AllLevels;

            level = level >= allLevels.Count ? allLevels.Count - 1 : level;

            LevelData currentData = allLevels[level];
            _currentLevel = level;

            AsyncOperation levelLoad = SceneManager.LoadSceneAsync("Level" + level, LoadSceneMode.Additive); // level

            levelLoad.completed += x =>
            {
                Hub.LevelGenerationCompleted.Fire(new LevelMetaData
                {
                    LevelData = currentData,
                    ActualLevelIndex = level,
                    VisualLevelIndex = visualLevelIndex
                });
            };
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
        }
#endif
    }
}
using Data;
using Data.Levels;
using DuelSystem;
using SignalsFramework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

public class CollectedBulletsCounter : MonoBehaviour
{
    [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

    #region "Signals"

    public static readonly Subject<int> OnCountIncreased = new Subject<int>();
    public static readonly Subject<int> OnCountDecreased = new Subject<int>();

    #endregion

    #region "Properties"

    public int CollectedBulletsCount => _collectedBulletsCount;

    #endregion

    #region "Fields"

    private int _collectedBulletsCount = 0;
    private int _limitBulletsCount = 0;

    private bool _isAvoidingDecreasing = false;

    #endregion

    private void Awake()
    {
        Hub.LevelGenerationCompleted.Subscribe(levelMetaData => InitSettings(levelMetaData.LevelData)).AddTo(_ltt);

        CollectibleBullet.OnBulletCollected.Where(_ => _collectedBulletsCount < _limitBulletsCount).Subscribe(_ => IncreaseCountOfCollectedBullets()).AddTo(_ltt);
        DuelResultController.OnDuelResultChecked.Where(duelResult => duelResult == DuelResultType.Win).Subscribe(_ => DecreaseCountOfCollectedBullets()).AddTo(_ltt);
        PowerProgressHandler.OnPowerCharged.Subscribe(_ => _isAvoidingDecreasing = true).AddTo(_ltt);
    }

    private void InitSettings(LevelData levelData)
    {
        _isAvoidingDecreasing = false;

        _limitBulletsCount = levelData.LimitBulletsCount;
        SetTargetBulletsCount(levelData.StartBulletsCount);
    }

    private void SetTargetBulletsCount(int targetBulletsCount)
    {
        _collectedBulletsCount = 0;

        for (int i = 0; i < targetBulletsCount; i++)
        {
            IncreaseCountOfCollectedBullets();
        }
    }

    private void IncreaseCountOfCollectedBullets()
    {
        _collectedBulletsCount++;
        CollectedBulletsCounter.OnCountIncreased.Fire(_collectedBulletsCount);
    }

    private void DecreaseCountOfCollectedBullets()
    {
        if (_isAvoidingDecreasing)
        {
            _isAvoidingDecreasing = false;
            return;
        }

        _collectedBulletsCount--;
        CollectedBulletsCounter.OnCountDecreased.Fire(_collectedBulletsCount);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
    }
#endif
}

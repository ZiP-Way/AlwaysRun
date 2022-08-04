using Data;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class BulletsCounterUI : MonoBehaviour
{
    [SerializeField] private Color _unactiveColor = default;
    [SerializeField] private Color _activeColor = default;

    [SerializeField] private TMP_Text _counterText = default;

    [SerializeField] private RectTransform _bulletRect = default;
    [SerializeField] private RectTransform _backgroundPanel = default;
    [SerializeField] private RectTransform _bulletRoot = default;

    [SerializeField] private float _interval = 20;

    [SerializeField, HideInInspector] private ObservableDestroyTrigger _ltt = default;

    #region "Fields"

    private List<RectTransform> _activeImages = default;
    private List<RectTransform> _unactiveImages = default;

    private float _currentPosY = 0;
    private float _bulletHeight = 0;

    #endregion

    public void Init()
    {
        Hub.LoadLevel.Subscribe(_ => ClearBullets()).AddTo(_ltt);

        CollectedBulletsCounter.OnCountIncreased.Subscribe(currentCount => CountIncreased(currentCount)).AddTo(_ltt);
        CollectedBulletsCounter.OnCountDecreased.Subscribe(currentCount => CountDecreased(currentCount)).AddTo(_ltt);

        _currentPosY = -50f;
        _bulletHeight = _bulletRect.sizeDelta.y;
    }

    private void CountIncreased(int currentCount)
    {
        if (_unactiveImages.Count > 0)
        {
            RectTransform bulletRect = _unactiveImages[0];
            _unactiveImages.RemoveAt(0);
            DoActiveBullet(bulletRect);
        }
        else
        {
            if(_activeImages.Count == 0) _backgroundPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _backgroundPanel.sizeDelta.y + 100);
            else _backgroundPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _backgroundPanel.sizeDelta.y + 80);

            RectTransform bulletTransform = GenerateBullet();
            bulletTransform.anchoredPosition = new Vector2(0, _currentPosY);

            _currentPosY -= _interval;
            _currentPosY -= _bulletHeight;

            DoActiveBullet(bulletTransform);
        }

        _counterText.text = currentCount.ToString();
    }

    private void CountDecreased(int currentCount)
    {
        if (_activeImages.Count <= 0) return;

        RectTransform bulletRect = _activeImages[_activeImages.Count - 1];
        _activeImages.RemoveAt(_activeImages.Count - 1);
        DoUnactiveBullet(bulletRect);

        if(currentCount > _activeImages.Count)
        {
            CountIncreased(currentCount);
        }

        _counterText.text = currentCount.ToString();
    }

    private void DoActiveBullet(RectTransform bulletRect)
    {
        bulletRect.GetComponent<Image>().color = _activeColor;
        _activeImages.Add(bulletRect);
    }

    private void DoUnactiveBullet(RectTransform bulletRect)
    {
        bulletRect.GetComponent<Image>().color = _unactiveColor;
        _unactiveImages.Add(bulletRect);
    }

    private RectTransform GenerateBullet()
    {
        GameObject bullet = Instantiate(_bulletRect.gameObject, _bulletRoot);
        RectTransform bulletRectTransform = bullet.GetComponent<RectTransform>();
        return bulletRectTransform;
    }

    private void ClearBullets()
    {
        if (_activeImages != null)
        {
            for (int i = 0; i < _activeImages.Count; i++)
            {
                Destroy(_activeImages[i].gameObject);
            }
        }

        if (_unactiveImages != null)
        {
            for (int i = 0; i < _unactiveImages.Count; i++)
            {
                Destroy(_unactiveImages[i].gameObject);
            }
        }

        _currentPosY = -50f;
        _backgroundPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

        _activeImages = new List<RectTransform>();
        _unactiveImages = new List<RectTransform>();

        _counterText.text = "0";
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_ltt == null) _ltt = gameObject.SetupDestroyTrigger();
    }
#endif
}
using UI.ProgressBar;
using UnityEngine;
using UnityEngine.UI;

public class PowerMakerUI : MonoBehaviour
{
    [SerializeField] private ProgressBarUI _progressBar = default;

    [SerializeField] private Image _powerIcon = default;
    [SerializeField] private Color _iconUnactiveColor = default;
    [SerializeField] private Color _iconActiveColor = default;
    [Space]
    [SerializeField] private Image _iconBackground = default;
    [SerializeField] private Color _iconBackgroundUnactiveColor = default;
    [SerializeField] private Color _iconBackgroundActiveColor = default;

    public void Fill(float progress)
    {
        _progressBar.ProgressChanged(progress);
    }

    public void Enable()
    {
        _powerIcon.color = _iconActiveColor;
        _iconBackground.color = _iconBackgroundActiveColor;
    }

    public void Disable()
    {
        _powerIcon.color = _iconUnactiveColor;
        _iconBackground.color = _iconBackgroundUnactiveColor;
        _progressBar.ProgressChanged(0.0f, true);
    }
}

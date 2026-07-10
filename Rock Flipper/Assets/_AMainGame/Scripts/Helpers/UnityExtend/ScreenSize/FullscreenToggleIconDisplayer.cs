using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggleIconDisplayer : ValueDisplayer<bool>
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Sprite fullscreenOnIcon;
    [SerializeField]
    private Sprite fullscreenOffIcon;

    protected override void Display(bool isFirst, bool previousValue, bool currentValue)
    {
        iconImage.sprite = currentValue ? fullscreenOnIcon : fullscreenOffIcon;
    }

    protected override bool GetCurrentValue()
    {
        return FullscreenToggle.IsWindowedMode();
    }
}

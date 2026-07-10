using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalMenuListItemDot : HorizontalMenuListItemBase
{
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private float lockedAlpha;
    [SerializeField]
    private float unlockedAlpha = 1;
    [SerializeField]
    private Sprite lockedSprite;
    [SerializeField]
    private Sprite unlockedSprite;

    protected override void OnEnable()
    {
        ///
        base.OnEnable();

        ///
        UpdateView();
    }

    private void UpdateView()
    {
        ///
        if (horizontalMenuItem == null)
        {
            return;
        }

        ///
        if (horizontalMenuItem.IsUnlocked)
        {
            ///
            iconImage.SetAlpha(unlockedAlpha);

            ///
            iconImage.sprite = unlockedSprite;
        }
        else
        {
            ///
            iconImage.SetAlpha(lockedAlpha);

            ///
            iconImage.sprite = lockedSprite;
        }
    }

    protected override void OnSetMenuItemHandler()
    {
        ///
        base.OnSetMenuItemHandler();

        ///
        UpdateView();
    }

    protected override void HorizontalMenuItem_OnUnlockStateChanged()
    {
        ///
        base.HorizontalMenuItem_OnUnlockStateChanged();

        ///
        UpdateView();
    }
}

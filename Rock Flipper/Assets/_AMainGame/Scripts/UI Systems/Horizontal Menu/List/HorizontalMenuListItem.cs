using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HorizontalMenuListItem : HorizontalMenuListItemBase
{
    [SerializeField]
    private UnifiedText titleText;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private GameObject notificationBadgeObject;
    [SerializeField]
    private UnifiedText notificationCountText;

    [Space]
    [SerializeField]
    protected float lockedAlpha = 0.5f;
    [SerializeField]
    protected string lockedTitle = "[LOCKED]";
    [SerializeField]
    protected Sprite lockedSprite;

    [Space]
    [SerializeField]
    private UnityEvent onClickedOnUnlockedItem;
    [SerializeField]
    private UnityEvent onClickedOnLockedItem;

    protected override void OnEnable()
    {
        ///
        base.OnEnable();

        ///
        ViewIconAndTitle();

        ///
        SetNotificationCount();
    }

    protected override void HorizontalMenuItem_OnUnlockStateChanged()
    {
        ///
        base.HorizontalMenuItem_OnUnlockStateChanged();

        ///
        ViewIconAndTitle();
    }

    protected override void OnSetMenuItemHandler()
    {
        ///
        base.OnSetMenuItemHandler();

        ///
        ViewIconAndTitle();

        ///
        SetNotificationCount();
    }

    public virtual void ViewIconAndTitle()
    {
        ///
        if (horizontalMenuItem == null)
        {
            return;
        }

        if (horizontalMenuItem.IsUnlocked)
        {
            ///
            titleText.SetAlpha(1);
            iconImage.SetAlpha(1);

            ///
            titleText.Text = horizontalMenuItem.Title;
            iconImage.sprite = horizontalMenuItem.Icon;
        }
        else
        {
            ///
            titleText.SetAlpha(lockedAlpha);
            iconImage.SetAlpha(lockedAlpha);

            ///
            titleText.Text = lockedTitle;
            iconImage.sprite = lockedSprite;
        }
    }

    public virtual void HandleClick()
    {
        if (horizontalMenuItem.IsUnlocked)
        {
            ///
            HorizontalMenu.CurrentMenuItemId = ItemId;

            ///
            onClickedOnUnlockedItem?.Invoke();
        }
        else
        {
            onClickedOnLockedItem?.Invoke();
        }
    }

    private void SetNotificationCount()
    {
        ///
        if (HorizontalMenu == null)
        {
            return;
        }

        ///
        var horizontalMenuItem = HorizontalMenu.GetMenuItem(ItemId);
        var modifier = horizontalMenuItem.HorizontalMenuListItemModifier;
        int count = modifier != null ? modifier.NotificationCount : 0;

        ///
        SetNotificationCount(count);
    }

    public override void SetNotificationCount(int count)
    {
        ///
        if (!HorizontalMenuItem.IsUnlocked)
        {
            count = 0;
        }

        ///
        if (count == 0)
        {
            notificationBadgeObject.SetActive(false);
        }
        else if (count > 0)
        {
            notificationBadgeObject.SetActive(true);
            notificationCountText.Text = count < 99 ? count.ToString() : "99+";
        }
        else
        {
            notificationBadgeObject.SetActive(true);
            notificationCountText.Text = "";
        }
    }
}

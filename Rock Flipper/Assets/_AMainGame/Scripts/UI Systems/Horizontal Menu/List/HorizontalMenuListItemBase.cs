using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HorizontalMenuListItemBase : ExtendedMonoBehaviour
{
    public event System.Action OnSetMenuItem;

    [SerializeField]
    private bool useModifier = true;

    [Space]
    [SerializeField]
    private UnityEvent onViewLockedItem;
    [SerializeField]
    private UnityEvent onViewUnlockedItem;

    protected HorizontalMenuListItemModifier horizontalMenuListItemModifier;
    protected HorizontalMenuItem horizontalMenuItem;

    public HorizontalMenu HorizontalMenu { get; private set; }

    public int ItemId { get; private set; }

    public HorizontalMenuItem HorizontalMenuItem => HorizontalMenu?.GetMenuItem(ItemId);

    protected virtual void HorizontalMenuItem_OnUnlockStateChanged()
    {
        ViewLockState();
    }
    protected virtual void OnSetMenuItemHandler() { }

    protected virtual void OnEnable()
    {
        ///
        horizontalMenuListItemModifier?.OnItemEnabled(this);

        ///
        if (horizontalMenuItem != null)
        {
            ///
            ViewLockState();

            ///
            horizontalMenuItem.OnUnlockStateChanged += HorizontalMenuItem_OnUnlockStateChanged;
        }
    }

    protected virtual void OnDisable()
    {
        ///
        horizontalMenuListItemModifier?.OnItemDisabled();

        ///
        if (horizontalMenuItem != null)
        {
            horizontalMenuItem.OnUnlockStateChanged -= HorizontalMenuItem_OnUnlockStateChanged;
        }
    }

    protected void Update()
    {
        horizontalMenuListItemModifier?.OnItemUpdate();
    }

    public void SetMenuItem(HorizontalMenu horizontalMenu, int itemId)
    {
        ///
        var savedHorizontalMenu = HorizontalMenu;

        ///
        HorizontalMenu = horizontalMenu;
        ItemId = itemId;

        ///
        if (horizontalMenuItem != null)
        {
            horizontalMenuItem.OnUnlockStateChanged -= HorizontalMenuItem_OnUnlockStateChanged;
        }

        ///
        horizontalMenuItem = horizontalMenu.GetMenuItem(itemId);

        ///
        if (isActiveAndEnabled)
        {
            horizontalMenuItem.OnUnlockStateChanged += HorizontalMenuItem_OnUnlockStateChanged;
        }

        ///
        horizontalMenuListItemModifier = useModifier ? horizontalMenuItem.HorizontalMenuListItemModifier : null;

        ///
        if (savedHorizontalMenu == null)
        {
            horizontalMenuListItemModifier?.OnItemEnabled(this);
        }

        ///
        OnSetMenuItemHandler();

        ///
        OnSetMenuItem?.Invoke();

        ///
        ViewLockState();
    }

    private void ViewLockState()
    {
        ///
        if (horizontalMenuItem == null)
        {
            return;
        }

        ///
        if (horizontalMenuItem.IsUnlocked)
        {
            onViewUnlockedItem?.Invoke();
        }
        else
        {
            onViewLockedItem?.Invoke();
        }
    }

    public virtual void SetNotificationCount(int count) { }
}

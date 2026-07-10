using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalMenuHeaderViewWithScrollBar : HorizontalMenuHeaderView
{
    [Header("HorizontalMenuHeaderViewWithScrollBar")]
    [SerializeField]
    private UnifiedText currentMenuItemTitle;
    [SerializeField]
    private UnifiedText previousMenuItemTitle;
    [SerializeField]
    private UnifiedText nextMenuItemTitle;

    [Space]
    [SerializeField]
    private Scrollbar scrollbar;

    public override void UpdateHeader()
    {
        ///
        var menuItem = horizontalMenu.CurrentMenuItem;

        ///
        currentMenuItemTitle.Text = menuItem.Title;
        previousMenuItemTitle.Text = horizontalMenu.PreviousMenuItem.Title;
        nextMenuItemTitle.Text = horizontalMenu.NextMenuItem.Title;

        ///
        scrollbar.value = horizontalMenu.MenuItemCount == 1 ? 1 : horizontalMenu.CurrentMenuItemId / (float)(horizontalMenu.MenuItemCount - 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HorizontalMenuListItem))]
public class HorizontalMenuListItemHighlighter : MonoBehaviour
{
    [SerializeField]
    private UnityEvent highlightDelegate;
    [SerializeField]
    private UnityEvent unhighlightDelegate;

    private HorizontalMenuListItem horizontalMenuListItem;
    private HorizontalMenu horizontalMenu;

    protected void Awake()
    {
        ///
        horizontalMenuListItem = GetComponent<HorizontalMenuListItem>();

        ///
        UpdateMenuItem();

        ///
        horizontalMenuListItem.OnSetMenuItem += HorizontalMenuListItem_OnSetMenuItem;
    }

    private void HorizontalMenuListItem_OnSetMenuItem()
    {
        UpdateMenuItem();
    }

    private void HorizontalMenu_OnCurrentMenuItemChanged()
    {
        UpdateHighlightState();
    }

    private void UpdateMenuItem()
    {
        ///
        if (horizontalMenu != null)
        {
            horizontalMenu.OnCurrentMenuItemChanged -= HorizontalMenu_OnCurrentMenuItemChanged;
        }

        ///
        horizontalMenu = horizontalMenuListItem.HorizontalMenu;

        ///
        UpdateHighlightState();

        ///
        if (horizontalMenu != null)
        {
            horizontalMenu.OnCurrentMenuItemChanged += HorizontalMenu_OnCurrentMenuItemChanged;
        }
    }

    private void UpdateHighlightState()
    {
        ///
        if (horizontalMenu == null)
        {
            /// 
            unhighlightDelegate?.Invoke();

            ///
            return;
        }

        ///
        if (horizontalMenu.CurrentMenuItemId == horizontalMenuListItem.ItemId)
        {
            highlightDelegate?.Invoke();
        }
        else
        {
            unhighlightDelegate?.Invoke();
        }
    }
}

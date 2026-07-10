using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VerticalMenu : ExtendedMonoBehaviour
{
    [SerializeField]
    private List<VerticalMenuItem> menuItems;

    [SerializeField]
    private UnityEvent havingSelectedItemView;
    [SerializeField]
    private UnityEvent noSelectedItemView;

    private HashSet<GameObject> menuItemSet = new HashSet<GameObject>();

    private VerticalMenuItem activeMenuItem;

    protected override void ExtendedAwake()
    {
        foreach (var item in menuItems)
        {
            menuItemSet.Add(item.gameObject);
        }
    }

    public void OnEnable()
    {
        ///
        UpdateView();

        ///
        entry.uiSelectedEventManager.OnSelectionChanged += UiSelectedEventManager_OnSelectionChanged;
    }

    public void OnDisable()
    {
        ///
        entry.uiSelectedEventManager.OnSelectionChanged -= UiSelectedEventManager_OnSelectionChanged;
    }

    private void UiSelectedEventManager_OnSelectionChanged()
    {
        UpdateView();
    }

    private void UpdateView()
    {
        var currentSelectedGameObject = entry.uiSelectedEventManager.LastGameObject;
        VerticalMenuItem selectedMenuItem = currentSelectedGameObject != null ? currentSelectedGameObject.GetComponent<VerticalMenuItem>() : null;
        bool anyMenuItemSelected = selectedMenuItem != null ? menuItemSet.Contains(currentSelectedGameObject) : false;

        ///
        if (anyMenuItemSelected)
        {
            ///
            foreach (var menuItem in menuItems)
            {
                ///
                menuItem.IsActiveMenu = menuItem == selectedMenuItem;

                ///
                if (menuItem.IsActiveMenu)
                {
                    activeMenuItem = menuItem;
                }
            }

            ///
            EnableAllMenuItems();

            ///
            havingSelectedItemView?.Invoke();
        }
        else
        {
            ///
            DisableAllInactiveMenuItems();

            ///
            noSelectedItemView?.Invoke();
        }
    }

    private void EnableAllMenuItems()
    {
        foreach (var item in menuItems)
        {
            item.GetComponent<Selectable>().interactable = true;
        }
    }

    private void DisableAllInactiveMenuItems()
    {
        foreach (var item in menuItems)
        {
            item.GetComponent<Selectable>().interactable = item.IsActiveMenu;
        }
    }

    [ContextMenu("SetActiveItemAsSelected")]
    public void SetActiveItemAsSelected()
    {
        ///
        if (activeMenuItem == null)
        {
            return;
        }

        ///
        entry.uiSelectedEventManager.SetCurrentSelectedGameObject(activeMenuItem.gameObject);
    }

    [ContextMenu("SwitchToCurrentSection")]
    public void SwitchToCurrentSection()
    {
        ///
        if (activeMenuItem == null)
        {
            return;
        }

        ///        
        var selectable = activeMenuItem.SectionContent.GetComponentInChildren<Selectable>(false);

        ///
        if (selectable != null)
        {
            entry.uiSelectedEventManager.SetCurrentSelectedGameObject(selectable.gameObject);
        }
    }
}

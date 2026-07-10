using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HorizontalMenuListView : MonoBehaviour
{
    [SerializeField]
    private HorizontalMenuListItemBase firstListItem;
    [SerializeField]
    private HorizontalMenu horizontalMenu;

    [Space]
    [SerializeField]
    private bool doNotSelectMenuItem = false;

    private List<HorizontalMenuListItemBase> listItems;

    public void Awake()
    {
        ///
        listItems = new List<HorizontalMenuListItemBase>();

        ///
        listItems.Add(firstListItem);

        ///
        while (listItems.Count < horizontalMenu.MenuItemCount)
        {
            var listItem = Instantiate(firstListItem, firstListItem.transform.parent);
            listItems.Add(listItem);
        }

        ///
        for (int i = 0; i < listItems.Count; i++)
        {
            listItems[i].SetMenuItem(horizontalMenu, i);
        }
    }

    public void OnEnable()
    {
        ///
        SelectCurrentMenuItem();
    }

    private void SelectCurrentMenuItem()
    {
        ///
        if (doNotSelectMenuItem)
        {
            return;
        }

        ///
        var currentSelectedItem = listItems[horizontalMenu.CurrentMenuItemId];
        Entry.Instance.uiSelectedEventManager.SetCurrentSelectedGameObject(currentSelectedItem.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalMenu : ExtendedMonoBehaviour
{
    public event System.Action OnUpdateHeader;
    public event System.Action OnCurrentMenuItemChanged;

    [Space]
    [SerializeField]
    private HorizontalMenuItem firstMenuItem;

    protected List<HorizontalMenuItem> menuItems;

    private int firstMenuItemId;
    private int currentMenuItemId = -1;
    private int previousMenuItemId = -1;
    private int nextMenuItemId = -1;

    public int MenuItemCount
    {
        get
        {
            ///
            TryInit();

            ///
            return menuItems.Count;
        }
    }

    public int FirstMenuItemId
    {
        get
        {
            ///
            TryInit();

            ///
            return firstMenuItemId;
        }

        private set
        {
            firstMenuItemId = value;
        }
    }

    public HorizontalMenuItem CurrentMenuItem => GetMenuItem(CurrentMenuItemId);
    public HorizontalMenuItem NextMenuItem => GetMenuItem(nextMenuItemId);
    public HorizontalMenuItem PreviousMenuItem => GetMenuItem(previousMenuItemId);

    public int CurrentMenuItemId
    {
        get
        {
            ///
            TryInit();

            ///
            return currentMenuItemId;
        }

        set
        {
            ///
            if (currentMenuItemId != value)
            {
                ///
                HorizontalMenuItem oldMenuItem = currentMenuItemId >= 0 ? menuItems[currentMenuItemId] : null;

                ///
                if (oldMenuItem != null)
                {
                    oldMenuItem.HideContent();
                }

                ///
                var newMenuItem = menuItems[value];

                ///
                newMenuItem.ShowContent();

                ///
                currentMenuItemId = value;

                ///
                UpdateNextAndPreviousItemId();

                ///
                OnUpdateHeader?.Invoke();
                OnCurrentMenuItemChanged?.Invoke();
            }
        }
    }

    private void UpdateNextAndPreviousItemId()
    {
        nextMenuItemId = GetNextMenuItemId();
        previousMenuItemId = GetPreviousMenuItemId();
    }

    public HorizontalMenuItem GetMenuItem(int id)
    {
        return menuItems[id];
    }

    protected override bool Init()
    {
        ///
        GetMenuItemList();

        ///
        HideAllContents();

        ///
        CurrentMenuItemId = FirstMenuItemId = GetFirstMenuItemId();


        ///
        return true;
    }


    private void HideAllContents()
    {
        foreach (var item in menuItems)
        {
            item.HideContent();
        }
    }

    private void GetMenuItemList()
    {
        ///
        menuItems = menuItems ?? new List<HorizontalMenuItem>();

        ///
        GetComponentsInChildren(menuItems);
    }

    private int GetFirstMenuItemId()
    {
        ///
        if (firstMenuItem == null)
        {
            return 0;
        }

        ///
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (menuItems[i] == firstMenuItem)
            {
                return i;
            }
        }

        ///
        return 0;
    }

    public void MoveNext()
    {
        CurrentMenuItemId = nextMenuItemId;
    }

    public void MoveBack()
    {
        CurrentMenuItemId = previousMenuItemId;
    }

    private int GetNextMenuItemId()
    {
        ///
        int id = currentMenuItemId + 1;

        ///
        while (true)
        {
            ///
            if (id >= menuItems.Count)
            {
                id = 0;
            }

            ///
            if (GetMenuItem(id).IsUnlocked)
            {
                return id;
            }

            ///
            id++;
        }

        ///
        throw new System.Exception("Can not determine next menu item's Id");
    }

    private int GetPreviousMenuItemId()
    {
        ///
        int id = currentMenuItemId - 1;

        ///
        while (true)
        {
            ///
            if (id < 0)
            {
                id = menuItems.Count - 1;
            }

            ///
            if (GetMenuItem(id).IsUnlocked)
            {
                return id;
            }

            ///
            id--;
        }

        ///
        throw new System.Exception("Can not determine previous menu item's Id");
    }
}

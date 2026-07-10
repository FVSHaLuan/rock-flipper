using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HorizontalMenuHeaderView : MonoBehaviour
{
    [SerializeField]
    protected HorizontalMenu horizontalMenu;

    public abstract void UpdateHeader();

    protected virtual void OnEnable()
    {
        ///
        if (horizontalMenu.CurrentMenuItemId < 0)
        {
            return;
        }

        ///
        UpdateHeader();

        ///
        horizontalMenu.OnUpdateHeader += HorizontalMenu_OnUpdateHeader;
    }

    protected virtual void OnDisable()
    {
        horizontalMenu.OnUpdateHeader -= HorizontalMenu_OnUpdateHeader;
    }

    private void HorizontalMenu_OnUpdateHeader()
    {
        UpdateHeader();
    }
}

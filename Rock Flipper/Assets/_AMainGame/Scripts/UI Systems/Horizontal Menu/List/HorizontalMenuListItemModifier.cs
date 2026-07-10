using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMenuListItemModifier : ExtendedMonoBehaviour
{
    public virtual int NotificationCount => 0;

    public virtual void OnItemEnabled(HorizontalMenuListItemBase horizontalMenuListItem) { }

    public virtual void OnItemDisabled() { }

    public virtual void OnItemUpdate() { }

    protected void OnEnable()
    {
        enabled = false;
    }
}

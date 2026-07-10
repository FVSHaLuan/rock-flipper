using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SelectableExtentions
{
    public static void SetInteractable(this Selectable selectable, bool interactable)
    {
        ///
        UIInputActionBase uiInputActionBase = selectable as UIInputActionBase;

        ///
        if (uiInputActionBase != null)
        {
            uiInputActionBase.interactable = interactable;
        }
        else
        {
            selectable.interactable = interactable;
        }
    }
}

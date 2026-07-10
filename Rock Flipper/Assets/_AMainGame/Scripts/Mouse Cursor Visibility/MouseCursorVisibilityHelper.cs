using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorVisibilityHelper : MonoBehaviour
{
    public bool ShowEvenOnGamePad
    {
        set
        {
            Entry.Instance.mouseCursorVisibilityManager.ShowEvenOnGamePad = value;
        }
    }

    [System.Obsolete]
    public void AddHideLock()
    {
        
    }

    [System.Obsolete]
    public void RemoveHideLock()
    {
        
    }

    public void AddShowLock()
    {
        Entry.Instance.mouseCursorVisibilityManager.AddShowMouseCursorLock(this);
    }

    public void RemoveShowLock()
    {
        Entry.Instance.mouseCursorVisibilityManager.RemoveShowMouseCursorLock(this);
    }
}

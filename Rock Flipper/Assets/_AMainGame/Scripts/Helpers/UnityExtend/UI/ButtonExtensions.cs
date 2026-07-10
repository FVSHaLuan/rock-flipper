using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ButtonExtensions
{
    public static void SetAllOnclickPersistentListenerState(this Button button, UnityEventCallState state)
    {
        for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
        {
            button.onClick.SetPersistentListenerState(i, state);
        }
    }
}

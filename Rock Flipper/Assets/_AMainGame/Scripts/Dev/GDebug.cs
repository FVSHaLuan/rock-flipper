using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GDebug
{
    public static void DoIfEnabledCheat(System.Action action)
    {
        if (IsCheatEnabled())
        {
            action?.Invoke();
        }
    }

    public static void LogIfEnabledCheat(string str)
    {
        if (IsCheatEnabled())
        {
            Debug.Log(str);
        }
    }

    private static bool IsCheatEnabled()
    {
        return Entry.Instance.gameSettingObject.Data.enabledTerminal;
    }
}

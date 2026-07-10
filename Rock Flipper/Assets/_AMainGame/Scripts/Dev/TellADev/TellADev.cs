using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TellADev
{
    public static void That(string message)
    {
        Entry.Instance.tellADevPopup.Show(message);
    }

    public static void That(string messageFormat, params object[] objects)
    {
        Entry.Instance.tellADevPopup.Show(string.Format(messageFormat, objects));
    }
}

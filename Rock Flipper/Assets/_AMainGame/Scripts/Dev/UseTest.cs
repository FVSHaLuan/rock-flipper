using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public static class UseTest
{
    public static void Log(string testSettingName)
    {
        Debug.LogWarningFormat("Use_Test: {0}", testSettingName);
    }
}
#endif
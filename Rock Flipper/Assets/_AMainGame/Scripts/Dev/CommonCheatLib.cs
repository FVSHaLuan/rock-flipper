using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CommonCheatLib
{
    public static event System.Action<string> OnCheatSignalEmitted;

    public static bool DisplayDebugTexts { get; set; }
    public static bool IsInScreenshotMode =>
#if UNITY_EDITOR
        !notInScreenshotModeBalancer.IsBalanced;
#else
        false;
#endif         

    private static BalancerWithObjects notInScreenshotModeBalancer = new BalancerWithObjects();

    public static void AddScreenshotModeLock(object lockObject)
    {
        notInScreenshotModeBalancer.AddObject(lockObject);
    }

    public static void RemoveScreenshotModeLock(object lockObject)
    {
        notInScreenshotModeBalancer.RemoveObject(lockObject);
    }

    public static void EmitCheatSignal(string context)
    {
        ///
        OnCheatSignalEmitted?.Invoke(context);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DevSetting
{
    public static bool IsInternalEnvironment()
    {
        return System.IO.Directory.Exists("Overwritten_BurstDebugInformation_DoNotShip");
    }
}

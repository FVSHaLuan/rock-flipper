using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

[System.Obsolete]
public class LimitWidthOnWideScreen : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    public static void TryToLimit()
    {
        ///
        var currentRatio = (float)Screen.width / (float)Screen.height;

        ///
        if (currentRatio > ScreenSizeConfig.MaxWidthRatio)
        {
            Screen.SetResolution((int)(Screen.height * ScreenSizeConfig.MaxWidthRatio), Screen.height, Screen.fullScreenMode);
        }
        else if (currentRatio < ScreenSizeConfig.MinWidthRatio)
        {
            Screen.SetResolution(Screen.width, (int)(Screen.width / ScreenSizeConfig.MinWidthRatio), Screen.fullScreenMode);
        }
    }

    [ContextMenu("TryToLimit"), PlayModeOnly]
    private void _TryToLimit()
    {
        TryToLimit();
    }
}

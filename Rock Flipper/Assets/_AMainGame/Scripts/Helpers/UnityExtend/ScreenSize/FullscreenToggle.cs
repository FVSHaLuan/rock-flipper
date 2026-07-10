using UnityEngine;

public class FullscreenToggle : MonoBehaviour
{
    public void Toggle()
    {
        if (IsWindowedMode())
        {
            ScreenSizeConfig.SaveWindowedModeSize(Screen.width, Screen.height);
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            ScreenSizeConfig.GetWindowedModeSize(out int width, out int height);
            Screen.SetResolution(width, height, FullScreenMode.Windowed);
        }
    }

    public static bool IsWindowedMode()
    {
        return Screen.fullScreenMode == FullScreenMode.MaximizedWindow
            || Screen.fullScreenMode == FullScreenMode.Windowed;
    }
}

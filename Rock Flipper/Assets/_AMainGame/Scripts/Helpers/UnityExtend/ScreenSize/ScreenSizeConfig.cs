using UnityEngine;

public static class ScreenSizeConfig
{
    // Screens    
    public const float MinWidthRatio = 16f / 10f;
    public const float MaxWidthRatio = 19.5f / 9f;

    public static void SaveWindowedModeSize(int width, int height)
    {
        PlayerPrefs.SetInt("WindowedModeWidth", width);
        PlayerPrefs.SetInt("WindowedModeHeight", height);
        PlayerPrefs.Save();
    }

    public static void GetWindowedModeSize(out int width, out int height)
    {
        width = PlayerPrefs.GetInt("WindowedModeWidth", 1280);
        height = PlayerPrefs.GetInt("WindowedModeHeight", 720);
    }
}

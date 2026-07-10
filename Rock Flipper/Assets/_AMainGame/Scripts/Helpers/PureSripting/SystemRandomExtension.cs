using System;

public static class SystemRandomExtension
{
    public static int Range(this Random random, int min, int max)
    {
        return random.Next(min, max);
    }

    public static float Range(this Random random, float min, float max)
    {
        return UnityEngine.Mathf.Clamp((float)(random.NextDouble() * (max - min) + min), min, max);
    }
}

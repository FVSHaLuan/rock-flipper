using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtension
{
    public static Color OverrideAlpha(this Color color, float alpha)
    {
        var c = color;
        c.a = alpha;

        ///
        return c;
    }

    public static Vector3 ToHSVVector3(this Color color)
    {
        ///
        Color.RGBToHSV(color, out var h, out var s, out var v);

        ///
        return new Vector3(h, s, v);
    }

    public static float SqrHSVDistance(this Color color1, Color color2)
    {
        return (color1.ToHSVVector3() - color2.ToHSVVector3()).sqrMagnitude;
    }

    /// <summary>
    /// Wrap text with this color (i.e., <color=this>Text</color>)
    /// </summary>
    /// <param name="color"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string WrapText(this Color color, object text)
    {
        return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), text);
    }
}

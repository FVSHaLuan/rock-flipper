using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AlphaSetter
{
    public static void SetAlpha(this Graphic graphic, float alpha)
    {
        var color=graphic.color;
        color.a = alpha;
        graphic.color = color;
    }

    public static void SetAlpha(this UnifiedText graphic, float alpha)
    {
        var color = graphic.Color;
        color.a = alpha;
        graphic.Color = color;
    }
}

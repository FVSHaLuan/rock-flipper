using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DoubleHelper
{
    public static double Lerp(double a, double b, float t)
    {
        t = Mathf.Clamp01(t);
        return (b - a) * t + a;
    }

    public static double MoveTowards(double current, double target, double maxDelta)
    {
        double delta = System.Math.Abs(current - target);
        delta = System.Math.Min(delta, maxDelta);
        return target > current ? current + delta : current - delta;
    }
}

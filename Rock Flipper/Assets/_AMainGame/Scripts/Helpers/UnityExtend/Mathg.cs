using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mathg
{
    public const float TwoPi = Mathf.PI * 2;
    public const float EffectiveEpsilon = float.Epsilon * 2;
    public const float VectorEpsilon = 0.0001f;

    /// <summary>
    /// Cube root
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double Cbrt(double value)
    {
        return Math.Pow(value, 1.0d / 3.0d);
    }

    public static double RandomInRange(double inclusiveMin, double inclusiveMax)
    {
        ///
        var value = UnityEngine.Random.value;

        ///
        return Math.Abs(inclusiveMin - inclusiveMax) * value + Math.Min(inclusiveMin, inclusiveMax);
    }
    /// <summary>
    /// input: 0.2f, output: 80% 0, 20% 1
    /// input: 1.2f, output: 80% 1, 20% 2
    /// </summary>
    /// <param name="rate"></param>
    /// <returns></returns>
    public static int SettleRate(float rate)
    {
        ///
        int count = Mathf.FloorToInt(rate);

        ///
        if (UnityEngine.Random.value < (rate - count))
        {
            count++;
        }

        ///
        return count;
    }

    public static bool Approximately(double a, double b)
    {
        return EffectiveApproximately(a, b, double.Epsilon);
    }

    public static bool EffectiveApproximately(float a, float b)
    {
        return EffectiveApproximately(a, b, EffectiveEpsilon);
    }

    public static bool EffectiveApproximatelyForVector(float distanceA, float distanceB)
    {
        return EffectiveApproximately(distanceA, distanceB, VectorEpsilon);
    }

    public static double Lerp(double a, double b, float t)
    {
        return (b - a) * t + a;
    }

    public static double InverseLerp(double a, double b, double value)
    {
        ///
        var t = (value - a) / (b - a);

        ///
        if (t < 0)
        {
            return 0;
        }
        else if (t > 1)
        {
            return 1;
        }

        ///
        return t;
    }

    public static double InverseLerpWithoutClamp(double a, double b, double value)
    {
        ///
        var t = (value - a) / (b - a);

        ///
        return t;
    }

    public static float InverseLerpWithoutClamp(float a, float b, float value)
    {
        ///
        var t = (value - a) / (b - a);

        ///
        return t;
    }

    private static bool EffectiveApproximately(float a, float b, float epsilon)
    {
        return Math.Abs(a - b) <= epsilon;
    }

    private static bool EffectiveApproximately(double a, double b, double epsilon)
    {
        return Math.Abs(a - b) <= epsilon;
    }

    /// <summary>
    /// Scale vector to fit the box and keep aspect
    /// </summary>
    /// <param name="scalingVector"></param>
    /// <param name="box"></param>
    /// <returns></returns>
    public static Vector2 ScaleInSide(Vector2 scalingVector, Vector2 box)
    {
        ///
        var scaleX = box.x / scalingVector.x;
        var scaleY = box.y / scalingVector.y;

        ///
        return scalingVector * Math.Min(scaleY, scaleX);
    }

    /// <summary>
    /// randomly divides a value (>=0) into 2 parts
    /// </summary>
    /// <param name="value"></param>
    /// <param name="max1"></param>
    /// <param name="max2"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    public static void DivideValueRandomly(int value, int max1, int max2, out int value1, out int value2)
    {
        DivideValueRandomly(value, max1, max2, out value1, out value2, UnityRandom.Default);
    }

    /// <summary>
    /// randomly divides a value (>=0) into 2 parts
    /// </summary>
    /// <param name="value"></param>
    /// <param name="max1"></param>
    /// <param name="max2"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    public static void DivideValueRandomly(int value, int max1, int max2, out int value1, out int value2, IRandomGenerator random)
    {
#if UNITY_EDITOR
        ///
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException("value");
        }

        ///
        if (value > (max1 + max2))
        {
            throw new ArgumentOutOfRangeException("value must <= (max1 + max2)");
        }
#endif

        if (max1 > max2)
        {
            value1 = random.Range(Math.Max(value - max2, 0), Math.Min(max1, value) + 1);
            value2 = value - value1;
        }
        else
        {
            value2 = random.Range(Math.Max(value - max1, 0), Math.Min(max2, value) + 1);
            value1 = value - value2;
        }
    }

    public static float ClampAngle0360(float degreeAngle)
    {
        ///
        while (degreeAngle < 0)
        {
            degreeAngle += 360;
        }
        while (degreeAngle >= 360)
        {
            degreeAngle -= 360;
        }

        ///
        return degreeAngle;
    }

    /// <summary>
    /// Compute the angle of point in the circle whose center at origin.
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static float Angle(Vector2 origin, Vector2 point)
    {
        return Vector2.SignedAngle(Vector2.right, point - origin);
    }

    /// <summary>
    /// rotate point around origin in XY plane
    /// </summary>
    /// <param name="point"></param>
    /// <param name="angle">in degree</param>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static Vector2 RotatePoint(Vector2 point, float angle, Vector2 origin)
    {
        return (Vector2)(Quaternion.AngleAxis(angle, Vector3.forward) * (point - origin)) + origin;
    }

    public static Vector2 SetAngle(Vector2 point, float angle, Vector2 origin)
    {
        return (Vector2)(Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right) * (point - origin).magnitude + origin;
    }
}

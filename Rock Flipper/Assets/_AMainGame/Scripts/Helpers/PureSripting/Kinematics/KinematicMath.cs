using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KinematicMath
{
    public static bool GetTimeToReachTarget(float initialSpeed, float acceleration, float distance, out float time)
    {
        double solution1;
        double solution2;
        Ballistics.SolveQuadric(0.5f * acceleration, initialSpeed, -distance, out solution1, out solution2);

        ///
        if (!double.IsNaN(solution1) && solution1 >= 0)
        {
            ///
            time = (float)solution1;

            ///
            return true;
        }

        ///
        if (!double.IsNaN(solution2) && solution2 >= 0)
        {
            ///
            time = (float)solution2;

            ///
            return true;
        }

        ///
        time = float.NaN;
        return false;
    }

    public static float GetDeltaDistance(float initialSpeed, float acceleration, float time)
    {
        return initialSpeed * time + 0.5f * acceleration * time * time;
    }

    public static Vector2 GetDeltaPosition(Vector2 initialVelocity, Vector2 acceleration, float time)
    {
        ///
        var deltaX = GetDeltaDistance(initialVelocity.x, acceleration.x, time);
        var deltaY = GetDeltaDistance(initialVelocity.x, acceleration.x, time);

        ///
        return new Vector2(deltaX, deltaY);
    }
}

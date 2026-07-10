using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, Obsolete]
/// <summary>
/// A serializable 2D int vector
/// </summary>
public struct Vector2DInt : IComparable<Vector2DInt>
{
    public int x;
    public int y;

    public Vector2DInt(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    int IComparable<Vector2DInt>.CompareTo(Vector2DInt other)
    {
        ///
        if (other.x != x || other.y != y)
        {
            return 1;
        }

        ///
        return 0;
    }
}

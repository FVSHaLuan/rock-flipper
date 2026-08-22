using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Can ONLY tell if 2 Vector2 are the same or different
/// </summary>
public struct ComparableVector2 : IComparable<ComparableVector2>
{
    private Vector2 vector2;
    public Vector2 Vector2 => vector2;
    public float X => vector2.x;
    public float Y => vector2.y;

    public ComparableVector2(Vector2 vector2)
    {
        this.vector2 = vector2;
    }

    public ComparableVector2(float x, float y)
    {
        vector2 = new Vector2(x, y);
    }

    public int CompareTo(ComparableVector2 other)
    {
        if (other.X != X || other.Y != Y)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public static implicit operator Vector2(ComparableVector2 v)
    {
        return v.Vector2;
    }

    public static implicit operator ComparableVector2(Vector2 v)
    {
        return new ComparableVector2(v);
    }
}


/// <summary>
/// Can ONLY tell if 2 Vector2 are the same or different
/// </summary>
public struct ComparableVector2Int : IComparable<ComparableVector2Int>
{
    private Vector2Int vector2;
    public Vector2Int Vector2 => vector2;
    public int X => vector2.x;
    public int Y => vector2.y;

    public ComparableVector2Int(Vector2Int vector2)
    {
        this.vector2 = vector2;
    }

    public ComparableVector2Int(int x, int y)
    {
        vector2 = new Vector2Int(x, y);
    }

    public int CompareTo(ComparableVector2Int other)
    {
        if (other.X != X || other.Y != Y)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public static implicit operator Vector2Int(ComparableVector2Int v)
    {
        return v.Vector2;
    }

    public static implicit operator ComparableVector2Int(Vector2Int v)
    {
        return new ComparableVector2Int(v);
    }
}

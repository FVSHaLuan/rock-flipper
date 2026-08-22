using UnityEngine;

public static class DirectionExtensions
{
    public static Vector2 GetNormalizedVector(this Direction8 d)
    {
        switch (d)
        {
            case Direction8.Up:
                return Vector2.up;
            case Direction8.UpRight:
                return new Vector2(1, 1).normalized;
            case Direction8.Right:
                return Vector2.right;
            case Direction8.DownRight:
                return new Vector2(1, -1).normalized;
            case Direction8.Down:
                return Vector2.down;
            case Direction8.DownLeft:
                return new Vector2(-1, -1).normalized;
            case Direction8.Left:
                return Vector2.left;
            case Direction8.UpLeft:
                return new Vector2(-1, 1).normalized;
            default:
                throw new System.NotImplementedException();
        }
    }

    public static Vector2 GetVector(this Direction4 d)
    {
        switch (d)
        {
            case Direction4.Up:
                return Vector2.up;
            case Direction4.Right:
                return Vector2.right;
            case Direction4.Down:
                return Vector2.down;
            case Direction4.Left:
                return Vector2.left;
            default:
                throw new System.NotImplementedException();
        }
    }
}

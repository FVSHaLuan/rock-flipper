/// <summary>
/// starts from top (up), clockwise
/// </summary>
public enum Direction8
{
    Up = 0,
    UpRight = 1,
    Right = 2,
    DownRight = 3,
    Down = 4,
    DownLeft = 5,
    Left = 6,
    UpLeft = 7
}

public static class Direction8Extensions
{
    public static void GetVector(this Direction8 d, out float x, out float y)
    {
        switch (d)
        {
            case Direction8.Up:
                x = 0;
                y = 1;
                return;
            case Direction8.UpRight:
                x = 1;
                y = 1;
                return;
            case Direction8.Right:
                x = 1;
                y = 0;
                return;
            case Direction8.DownRight:
                x = 1;
                y = -1;
                return;
            case Direction8.Down:
                x = 0;
                y = -1;
                return;
            case Direction8.DownLeft:
                x = -1;
                y = -1;
                return;
            case Direction8.Left:
                x = -1;
                y = 0;
                return;
            case Direction8.UpLeft:
                x = -1;
                y = 1;
                return;
            default:
                throw new System.NotImplementedException();
        }
    }

    public static void GetVector(this Direction8 d, out int x, out int y)
    {
        switch (d)
        {
            case Direction8.Up:
                x = 0;
                y = 1;
                return;
            case Direction8.UpRight:
                x = 1;
                y = 1;
                return;
            case Direction8.Right:
                x = 1;
                y = 0;
                return;
            case Direction8.DownRight:
                x = 1;
                y = -1;
                return;
            case Direction8.Down:
                x = 0;
                y = -1;
                return;
            case Direction8.DownLeft:
                x = -1;
                y = -1;
                return;
            case Direction8.Left:
                x = -1;
                y = 0;
                return;
            case Direction8.UpLeft:
                x = -1;
                y = 1;
                return;
            default:
                throw new System.NotImplementedException();
        }
    }
}

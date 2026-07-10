/// <summary>
/// starts from top (up), clockwise
/// </summary>
public enum Direction4
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
}

public static class Direction4Extensions
{
    public static void GetVector(this Direction4 d, out float x, out float y)
    {
        switch (d)
        {
            case Direction4.Up:
                x = 0;
                y = 1;
                return;
            case Direction4.Right:
                x = 1;
                y = 0;
                return;
            case Direction4.Down:
                x = 0;
                y = -1;
                return;
            case Direction4.Left:
                x = -1;
                y = 0;
                return;
            default:
                throw new System.NotImplementedException();
        }
    }
    public static void GetVector(this Direction4 d, out int x, out int y)
    {
        float fx, fy;
        d.GetVector(out fx, out fy);
        x = (int)fx;
        y = (int)fy;
    }   
}

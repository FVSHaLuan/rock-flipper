public static class TimeStringHelper
{
    public static void GetComponentsFromSeconds(this double totalSeconds, out int hours, out int minutes, out int seconds)
    {
        ///
        hours = (int)(totalSeconds / 3600);
        totalSeconds -= hours * 3600;
        minutes = (int)(totalSeconds / 60);
        totalSeconds -= minutes * 60;
        seconds = (int)totalSeconds;
    }

    public static string GetStringFromSeconds(this double totalSeconds)
    {
        ///
        int hours;
        int minutes;
        int seconds;

        ///
        GetComponentsFromSeconds(totalSeconds, out hours, out minutes, out seconds);

        ///
        return GetString(0, hours, minutes, seconds);
    }

    public static string GetString(int days, int hours, int minutes, int seconds)
    {
        ///
        if (days > 0)
        {
            return string.Format("{0}d{1:00}h{2:00}m{3:00}s", days, hours, minutes, seconds);
        }

        ///
        if (hours > 0)
        {
            return string.Format("{0}h{1:00}m{2:00}s", hours, minutes, seconds);
        }

        ///
        if (minutes > 0)
        {
            return string.Format("{0}m{1:00}s", minutes, seconds);
        }

        ///
        return string.Format("{0}s", seconds);
    }

    public static string GetStringFromSeconds(string format, double totalSeconds)
    {
        ///
        int hours;
        int minutes;
        int seconds;

        ///
        GetComponentsFromSeconds(totalSeconds, out hours, out minutes, out seconds);

        ///
        return string.Format(format, hours, minutes, seconds);
    }
}

public static class NumberOrdinalSuffix
{
    public static string GetOrdinalSuffix(int num)
    {
        if (num <= 0) return "";

        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return "th";
        }

        switch (num % 10)
        {
            case 1:
                return "st";
            case 2:
                return "nd";
            case 3:
                return "rd";
            default:
                return "th";
        }
    }

    public static string ToStringWithOrdinalSuffix(this int num)
    {
        return num.ToString() + GetOrdinalSuffix(num);
    }

    public static string ToOrdinalString(this int num)
    {
        switch (num)
        {
            case 0:
                return "zeroth";
            case 1:
                return "first";
            case 2:
                return "second";
            case 3:
                return "third";
            case 4:
                return "fourth";
            case 5:
                return "fifth";
            case 6:
                return "sixth";
            case 7:
                return "seventh";
            default:
                throw new System.NotImplementedException();
        }
    }
}

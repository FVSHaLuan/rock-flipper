using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToStringFast
{
    private const int MinValue = -999;
    private const int MaxValue = 9999;

    private static Dictionary<int, string> cachedIntStrings = new Dictionary<int, string>();

    public static string ToStringCached(this int number)
    {
        ///
        if (number > MaxValue || number < MinValue)
        {
            return number.ToString();
        }

        ///
        string str = "";

        ///
        if (!cachedIntStrings.TryGetValue(number, out str))
        {
            ///
            str = number.ToString();

            ///
            cachedIntStrings.Add(number, str);
        }

        ///
        return str;
    }
}

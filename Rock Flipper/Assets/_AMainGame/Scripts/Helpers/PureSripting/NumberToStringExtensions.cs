using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class NumberToStringExtensions
{
    private static List<string> verbalNumbers = new List<string>()
    {
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
    };

    private static readonly SortedDictionary<int, string> romanMap = new SortedDictionary<int, string>()
    {
        { 1000, "M" },
        { 900, "CM" },
        { 500, "D" },
        { 400, "CD" },
        { 100, "C" },
        { 90, "XC" },
        { 50, "L" },
        { 40, "XL" },
        { 10, "X" },
        { 9, "IX" },
        { 5, "V" },
        { 4, "IV" },
        { 1, "I" }
    };

    public static string ToRomanian(this int num)
    {
        if (num <= 0)
        {
            throw new ArgumentOutOfRangeException("Input must be a positive integer.");
        }

        StringBuilder roman = new StringBuilder();
        foreach (KeyValuePair<int, string> pair in romanMap.Reverse())
        {
            while (num >= pair.Key)
            {
                roman.Append(pair.Value);
                num -= pair.Key;
            }
        }
        return roman.ToString();
    }

    public static string ToVerbalNumberString(this int i)
    {
        return verbalNumbers[i];
    }

    public static string ToStringWithSign(this int i)
    {
        if (i > 0)
        {
            return "+" + i.ToString();
        }
        else if (i < 0)
        {
            return i.ToString();
        }
        else
        {
            return "0";
        }
    }

    public static string ToStringWithSignForce0(this int i)
    {
        if (i >= 0)
        {
            return "+" + i.ToString();
        }
        else if (i < 0)
        {
            return i.ToString();
        }
        else
        {
            return "0";
        }
    }
}

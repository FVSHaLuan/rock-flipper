using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StringHelper
{
    public static string Replace(this string str, int startIndex, int length, string newString)
    {
        ///
        string prefix = startIndex > 0 ? str.Substring(0, startIndex) : null;
        string suffix = (length + startIndex) < str.Length ? str.Substring(startIndex + length) : null;

        ///
        return prefix + newString + suffix;
    }

    public static string Replace(this string str, int startIndex, int length, char newChar)
    {
        return Replace(str, startIndex, length, newChar.ToString());
    }

    /// <summary>
    /// Adds sentence period.
    /// </summary>
    /// <param name="str"></param>
    /// <returns>String with period mark at the end.</returns>
    public static string AddEnglishPeriod(this string str)
    {
        return str.AddPeriod('.', true);
    }

    /// <summary>
    /// Adds sentence period.
    /// </summary>
    /// <param name="str"></param>
    /// <returns>String with period mark at the end.</returns>
    public static string AddPeriod(this string str, char period, bool isFormatted)
    {
        ///
        if (string.IsNullOrWhiteSpace(str))
        {
            return "";
        }

        ///
        if (str.HasPeriod(period, isFormatted))
        {
            return str;
        }
        else
        {
            return str + period;
        }
    }

    public static bool HasPeriod(this string str, char period, bool isFormatted)
    {
        ///
        if (string.IsNullOrWhiteSpace(str))
        {
            return false;
        }

        ///
        if (!isFormatted)
        {
            return str.Last() == period;
        }

        ///
        bool isInFormatBracket = false;
        for (int i = str.Length - 1; i >= 0; i--)
        {
            ///
            var c = str[i];

            ///
            if (!isInFormatBracket)
            {
                if (c == '>')
                {
                    isInFormatBracket = true;
                    continue;
                }

                ///
                if (c == ' '
                    || c == '\r'
                    || c == '\n')
                {
                    continue;
                }

                ///
                return c == period;
            }

            ///
            if (c == '<')
            {
                isInFormatBracket = false;
            }
        }

        ///
        return false;
    }

    public static string WithMaxLength(this string value, int maxLength)
    {
        if (value == null)
        {
            return null;
        }

        ///
        return value.Substring(0, Math.Min(value.Length, maxLength));
    }

    public static string HashWithSHA1(this string str)
    {
        if (String.IsNullOrEmpty(str))
        {
            return String.Empty;
        }

        // Uses SHA256 to create the hash
        using (var sha = new System.Security.Cryptography.SHA1Managed())
        {
            // Convert the string to a byte array first, to be processed
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = sha.ComputeHash(textBytes);

            // Convert back to a string, removing the '-' that BitConverter adds
            string hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", String.Empty);

            return hash;
        }
    }

    public static string SubstringClamped(this string str, int index, int maxLength)
    {
        ///
        if (str == null || str.Length == 0)
        {
            return str;
        }

        ///
        var length = Mathf.Min(maxLength, str.Length - index);
        return str.Substring(index, length);
    }
}

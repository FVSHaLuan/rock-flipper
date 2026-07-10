using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class FormattedStringHelper
{
    private static Regex NonalphanumericRegex = new Regex("[^a-zA-Z0-9 -]");
    private const char OpenTag = '<';
    private const char CloseTag = '>';

    public static int FindFirstUnformattedCharacter(string str)
    {
        ///
        int rs = -1;
        int isInTag = 0;

        ///
        for (int i = 0; i < str.Length; i++)
        {
            ///
            var ch = str[i];

            ///
            if (ch == OpenTag)
            {
                ///
                isInTag++;

                ///
                continue;
            }
            else if (ch == CloseTag)
            {
                ///
                isInTag--;

                ///
                continue;
            }

            ///
            if (isInTag <= 0)
            {
                return i;
            }
        }

        ///
        return rs;
    }

    public static string LowerCaseFirstUnformattedLetter(this string str)
    {
        ///
        var firstCharacterPos = FindFirstUnformattedCharacter(str);

        ///
        if (firstCharacterPos < 0)
        {
            return str;
        }

        ///
        var firstLetter = str[firstCharacterPos];

        ///
        if (!IsLetter(firstLetter))
        {
            return str;
        }

        ///
        if (char.IsLower(firstLetter))
        {
            return str;
        }

        ///
        var loweredLetter = char.ToLower(firstLetter);

        ///
        return str.Replace(firstCharacterPos, 1, loweredLetter);
    }

    public static bool IsLetter(char ch)
    {
        return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
    }

    public static string StripFormatAll(this string formattedString)
    {
        return formattedString.StripFormatAngleBracket().StripFormatSquareBracket();
    }

    public static string StripFormatAngleBracket(this string formattedString)
    {
        return Regex.Replace(formattedString, "<.*?>", string.Empty);
    }

    public static string StripFormatSquareBracket(this string formattedString)
    {
        return Regex.Replace(formattedString, "[.*?]", string.Empty);
    }

    public static string RemoveAllNonalphanumericCharacters(this string str)
    {
        return NonalphanumericRegex.Replace(str, "");
    }
}

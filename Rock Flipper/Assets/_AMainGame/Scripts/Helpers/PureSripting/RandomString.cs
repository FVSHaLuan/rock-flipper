using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class RandomString
{
    public const string AZ09 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private static StringBuilder stringBuilder = new StringBuilder();

    public static string GetRandomString(int length, string characters)
    {
        ///
        if (string.IsNullOrEmpty(characters))
        {
            throw new System.ArgumentException("GetRandomString: can not be empty or null");
        }

        ///
        stringBuilder.Clear();

        ///
        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(characters[Random.Range(0, characters.Length)]);
        }

        //
        return stringBuilder.ToString();
    }

    public static string GetRandomAlphabeticalString(int length)
    {
        ///
        stringBuilder.Clear();

        ///
        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(RandomAlphabeticalCharacter());
        }

        //
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Characters are picked randomly from '!' to '~'
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GetRandomReadableAsciiString(int length)
    {
        ///
        stringBuilder.Clear();

        ///
        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(RandomReadableAsciiCharacter());
        }

        //
        return stringBuilder.ToString();
    }

    private static char RandomAlphabeticalCharacter()
    {
        if (Random.value > 0.5f)
        {
            return (char)Random.Range('a', 'z');
        }
        else
        {
            return (char)Random.Range('A', 'Z');
        }
    }

    private static char RandomReadableAsciiCharacter()
    {
        return (char)Random.Range('!', '~');
    }
}

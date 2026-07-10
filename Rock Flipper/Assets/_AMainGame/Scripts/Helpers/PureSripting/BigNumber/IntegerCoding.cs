using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Encodes big number of other types to integer while keeps the original order (e.g: a < b then encoded a <  encoded b)
/// Max int = 2,147,483,647
/// Encoding format: [X,XX]Y,YYY,YYY which is equal to 10^[X,XX] * (1 + 9*Y,YYY,YYY / 10,000,000)
/// [X,XX]: tenPart
/// Y,YYY,YYY: fractionPart
/// </summary>
public static class IntegerCoding
{
    private const int TenMil = 10000000;
    private const int ElevenMil = TenMil * 10;

    public static int EncodeToInt(double bigNumber)
    {
        return EncodeToInt(bigNumber, TenMil, 214);
    }

    public static double DecodeToDouble(int encodedInt)
    {
        return DecodeToDouble(encodedInt, TenMil);
    }

    public static int EncodeToInt2(double bigNumber)
    {
        return EncodeToInt(bigNumber, ElevenMil, 214);
    }

    public static double DecodeToDouble2(int encodedInt2)
    {
        return DecodeToDouble(encodedInt2, ElevenMil);
    }

    private static int EncodeToInt(double bigNumber, int powerOfTen, int maxValueOfTenPart)
    {
        ///
        var absoluteBigNumber = Math.Abs(bigNumber);
        var sign = Math.Sign(bigNumber);

        ///
        if (absoluteBigNumber < 1)
        {
            return (int)absoluteBigNumber * sign;
        }

        ///
        int tenPart = (int)Math.Floor(Math.Log(absoluteBigNumber, 10));

        // Max value
        if (tenPart > maxValueOfTenPart)
        {
            return maxValueOfTenPart * powerOfTen;
        }

        ///
        var fractionPart = (int)((absoluteBigNumber / Math.Pow(10, tenPart) - 1) * powerOfTen / 9.0f);

        ///
        return tenPart * powerOfTen + fractionPart;
    }

    private static double DecodeToDouble(int encodedInt, int powerOfTen)
    {
        ///
        var tenPart = encodedInt / powerOfTen;
        var fractionPart = encodedInt - tenPart * powerOfTen;

        ///
        return Math.Pow(10.0f, tenPart) * (1.0f + 9.0f * fractionPart / (double)powerOfTen);
    }
}

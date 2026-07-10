using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class BigNumberString
{
    private const string ScientificModeKey = "BigNumberScientificMode";
    private static bool scientificMode;
    public static bool ScientificMode
    {
        get => scientificMode;
        set
        {
            if (scientificMode == value)
            {
                return;
            }
            scientificMode = value;
            PlayerPrefs.SetInt(ScientificModeKey, value ? 1 : 0);
        }
    }

    private const string InfiniteName = "Inf";
    private const string NotAvailableName = "NaN";
    private const char MaxSuffixChar = 'Z';
    private const char MinSuffixChar = 'A';

    private static NumberFormatInfo PrefixFormatInfo = new NumberFormatInfo()
    {
        NumberGroupSeparator = ",",
    };

    private static List<Suffix> suffixes = new List<Suffix>()
    {
        new Suffix("K",1E+4,1E+3),
        new Suffix("M",1E+6,1E+6),
        new Suffix("B",1E+9,1E+9),
        new Suffix("T",1E+12,1E+12),
    };

    private static char nextSuffix_1 = 'A';
    private static char nextSuffix_2 = 'A';
    private static int nextSuffixMinPower = 18;
    private static int nextSuffixFactorPower = 15;

    struct Suffix
    {
        public string name;
        public double min;
        public double factor;

        public Suffix(string name, double min, double factor)
        {
            this.name = name;
            this.min = min;
            this.factor = factor;
        }
    }

    static BigNumberString()
    {
        scientificMode = PlayerPrefs.GetInt(ScientificModeKey, 0) > 0;
    }

    public static string ToBigNumberString(long value)
    {
        return ToBigNumberString((double)value);
    }

    public static string ToBigNumberString(float value)
    {
        return ToBigNumberString((double)value);
    }

    public static string ToBigNumberString(int value)
    {
        return ToBigNumberString((double)value);
    }

    public static string ToBigNumberString(double value)
    {
        ///
#if UNITY_EDITOR
        if (ScientificMode && value >= 1000)
        {
            return ToExponentialString(value);
        }
#endif
        ///
        if (double.IsNaN(value))
        {
            return NotAvailableName;
        }

        ///
        if (double.IsInfinity(value))
        {
            return InfiniteName;
        }

        ///
        if (value > double.MaxValue)
        {
            return InfiniteName;
        }

        ///
        if (value < suffixes[0].min)
        {
            return GetPrefixNumber(value);
        }

        ///
        int i = 1;
        while (true)
        {
            ///
            if (suffixes.Count <= i)
            {
                AddNewSuffix();
            }

            ///
            if (value < suffixes[i].min)
            {
                ///
                var lastSuffix = suffixes[i - 1];

                ///
                return string.Format("{0}{1}", GetPrefixNumber(value / lastSuffix.factor), lastSuffix.name);
            }

            ///
            i++;
        }
    }

    private static void AddNewSuffix()
    {
        ///
        string suffixName = string.Concat(nextSuffix_1, nextSuffix_2);
        double min = System.Math.Pow(10, nextSuffixMinPower);
        double factor = System.Math.Pow(10, nextSuffixFactorPower);

        ///
        suffixes.Add(new Suffix(suffixName, min, factor));

        ///        
        if (nextSuffix_2 == MaxSuffixChar)
        {
            nextSuffix_2 = MinSuffixChar;
            nextSuffix_1++;
        }
        else
        {
            nextSuffix_2++;
        }
        nextSuffixMinPower += 3;
        nextSuffixFactorPower += 3;
    }

    private static string GetPrefixNumber(double value)
    {
        ///
        string format = "#,0.##";

        ///
        if (value < 10)
        {
            format = "#,0.###";
        }
        else if (value < 100)
        {
            format = "#,0.##";
        }
        else if (value < 1000)
        {
            format = "#,0.#";
        }
        else
        {
            format = "#,0";
        }

        ///
        return value.ToString(format, PrefixFormatInfo);
    }

    public static string ToExponentialString(this double value)
    {
        if (value == 0)
            return "0";
        if (double.IsNaN(value))
        {
            return NotAvailableName;
        }
        if (double.IsInfinity(value))
        {
            return InfiniteName;
        }
        var sign = value < 0 ? "-" : "";
        var absValue = System.Math.Abs(value);
        var exponent = (int)System.Math.Floor(System.Math.Log10(absValue));
        var mantissa = absValue / System.Math.Pow(10, exponent);
        return $"{sign}{mantissa:0.##}E{exponent}";
    }
}

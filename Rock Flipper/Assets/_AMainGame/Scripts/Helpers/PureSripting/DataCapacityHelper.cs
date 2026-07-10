using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataCapacityHelper
{
    private const double ByteToBit = 8;
    private const double KBToBit = ByteToBit * 1024;
    private const double MBToBit = KBToBit * 1024;
    private const double GBToBit = MBToBit * 1024;
    private const double TBToBit = GBToBit * 1024;
    private const double PBToBit = TBToBit * 1024;

    private static List<string> units = new List<string>() { "b", "B", "KB", "MB", "GB", "TB", "PB" };

    public static void GetCapacity(double bit, out double convertedValue, out int unit)
    {
        ///
        if (bit < ByteToBit)
        {
            convertedValue = bit;
            unit = 0;
        }
        else if (bit < KBToBit)
        {
            convertedValue = bit / ByteToBit;
            unit = 1;
        }
        else if (bit < MBToBit)
        {
            convertedValue = bit / KBToBit;
            unit = 2;
        }
        else if (bit < GBToBit)
        {
            convertedValue = bit / MBToBit;
            unit = 3;
        }
        else if (bit < TBToBit)
        {
            convertedValue = bit / GBToBit;
            unit = 4;
        }
        else if (bit < PBToBit)
        {
            convertedValue = bit / TBToBit;
            unit = 5;
        }
        else
        {
            convertedValue = bit / PBToBit;
            unit = 6;
        }
    }

    public static void GetCapacity(double bit, out double convertedValue, out string unit)
    {
        ///
        GetCapacity(bit, out convertedValue, out int unitId);

        ///
        unit = units[unitId];
    }
}

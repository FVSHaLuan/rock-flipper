using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LogStringLineByLine
{
    public static void LogLineByLine(this string str)
    {
        str.LogLineByLineFormat(null, null);
    }

    public static void LogLineByLineFormat(this string str, string format)
    {
        LogLineByLineFormat(str, format, null);
    }

    public static void LogLineByLineFormat(this string str, string format, Object unityObject)
    {
        using (StringReader sr = new StringReader(str))
        {
            ///
            string line;

            ///
            while ((line = sr.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(format))
                {
                    Debug.LogFormat(unityObject, format, line);
                }
                else
                {
                    Debug.Log(line, unityObject);
                }
            }
        }
    }
}

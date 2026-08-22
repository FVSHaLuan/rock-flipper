using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VersionUtility
{
    private static char[] separators = new char[] { ',', '.', '-', ' ' };

    private static string[] components;

    public static string WithoutBuildNumber { get; private set; }
    public static string BuildNumberString { get; private set; }

    static VersionUtility()
    {
        components = Application.version.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);

        ///
        WithoutBuildNumber = components[0];
        for (int i = 1; i < components.Length - 1; i++)
        {
            WithoutBuildNumber += "." + components[i];
        }

        ///
        BuildNumberString = components[components.Length - 1];
    }
}

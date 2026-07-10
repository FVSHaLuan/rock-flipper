using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.DedicatedServer;

public static class CommandLineArguments
{
    private static HashSet<string> argumentSet = new HashSet<string>();
    private static HashSet<string> loweredArgumentSet = new HashSet<string>();
    private static string[] arguments;

    static CommandLineArguments()
    {
        arguments = Environment.GetCommandLineArgs();

        ///
        if (arguments != null)
        {
            ///
            argumentSet = new HashSet<string>();
            loweredArgumentSet = new HashSet<string>();

            ///
            foreach (var item in arguments)
            {
                argumentSet.Add(item);
                loweredArgumentSet.Add(item.ToLower());
            }
        }
    }

    public static bool HasArgument(string argument)
    {
        ///
        if (argumentSet == null)
        {
            return false;
        }

        return argumentSet.Contains(argument);
    }
}

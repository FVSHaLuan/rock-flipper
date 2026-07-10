using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMouseAndKeyboardByLaunchArgument : ExtendedMonoBehaviour
{
    private const string Argument = "forcemk";

    protected void Start()
    {
        if (CommandLineArguments.HasArgument(Argument))
        {
            GameSetting.ForceMouseAndKeyboard = true;

            ///
            Debug.LogWarning("Forced mouse and keyboard");
        }
    }
}

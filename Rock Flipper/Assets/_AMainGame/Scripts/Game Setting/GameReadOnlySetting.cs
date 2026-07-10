using FH.Core.Architecture.WritableData;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameReadOnlySetting
{
    private const string SettingFileName = "Setting.ini";
    private const string InputSection = "Input";
    private const string ForceMouseAndKeyboardKey = "ForceMouseAndKeyboard";

    private static string SettingFilePath;

    public static bool ForcedMouseAndKeyboard { get; private set; } = false;

    static GameReadOnlySetting()
    {
        /////
        //SettingFilePath = PersistentFileHelper.GetPath(SettingFileName);

        /////
        //if (!File.Exists(SettingFilePath))
        //{
        //    return;
        //}

        /////
        //INIParser iniParser = new INIParser();
        //iniParser.Open(SettingFilePath);

        /////
        //ForcedMouseAndKeyboard = iniParser.ReadValue(InputSection, ForceMouseAndKeyboardKey, ForcedMouseAndKeyboard);

        /////
        //iniParser.Close();
    }
}

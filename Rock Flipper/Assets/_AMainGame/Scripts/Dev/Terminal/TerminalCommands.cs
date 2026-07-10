using CommandTerminal;
using I2.Loc;
using System;
using Agame;
using Agame.Dev;
using UnityEngine;
using UnityEngine.SceneManagement;
using Agame.Run;

internal static partial class TerminalCommands
{
    private static WeaponTemporaryInactiveLockObject weaponTemporaryInactiveLockObject = new WeaponTemporaryInactiveLockObject();
    private static ObjectWithName lockObject = new ObjectWithName("TerminalCommands");
    private static TimeScaleControlStandalone gameplaySlowDownController = new TimeScaleControlStandalone(TimeScaleControlType.Override, 0.2f);

    private class WeaponTemporaryInactiveLockObject
    {

    }

    #region DevPanel
    [RegisterCommand()]
    private static void ShowLogViewer(CommandArg[] args)
    {
        Reporter.Instance?.doShow();
    }

    [RegisterCommand(Help = "")]
    private static void ToggleFps(CommandArg[] args)
    {
        DevStatManager.Instance.fps.ToggleActiveState();
    }

    [RegisterCommand(Help = "")]
    private static void ToggleEnemyCount(CommandArg[] args)
    {
        DevStatManager.Instance.enemyCount.ToggleActiveState();
    }
    #endregion DevPanel

    #region Screen
    [RegisterCommand(Help = "int x, int y")]
    private static void SetScreenResolution(CommandArg[] args)
    {
        ///
        int x = args[0].Int;
        int y = args[1].Int;

        ///
        Screen.SetResolution(x, y, Screen.fullScreen);
    }

    [RegisterCommand(Help = "bool isFullScreen")]
    private static void SetFullScreen(CommandArg[] args)
    {
        ///
        bool isFullScreen = args[0].Bool;

        ///
        Screen.fullScreen = isFullScreen;
    }

    [RegisterCommand(Help = "int fullScreenMode \nExclusiveFullScreen = 0\nFullScreenWindow = 1 \nMaximizedWindow = 2 \nWindowed = 3")]
    private static void SetFullScreenMode(CommandArg[] args)
    {
        ///
        FullScreenMode fullScreenMode = (FullScreenMode)(args[0].Int);

        ///
        Screen.fullScreenMode = fullScreenMode;
    }
    #endregion

    #region Settings
    [RegisterCommand(Help = "")]
    private static void ToggleForceMouseAndKeyboard(CommandArg[] args)
    {
        GameSetting.ForceMouseAndKeyboard = !GameSetting.ForceMouseAndKeyboard;
        Debug.LogFormat("ForceMouseAndKeyboard = {0}", GameSetting.ForceMouseAndKeyboard);
    }

    [RegisterCommand(Help = "")]
    private static void TogglePauseWhenInactive(CommandArg[] args)
    {
        var v = !Entry.Instance.GameSetting.pauseWhenLostFocus;
        Entry.Instance.GameSetting.pauseWhenLostFocus = v;
        Entry.Instance.gameSettingObject.SaveData();

        ///
        Debug.LogFormat("Saved Setting! Auto pause when inactive = {0}", v);
    }

    [RegisterCommand(Help = "float value")]
    private static void SetDarkness(CommandArg[] args)
    {
        ///
        float value = args[0].Float;

        ///
        Debug.LogFormat("Current darkness: {0}", Entry.Instance.GameSetting.LightDarkness);

        ///
        Entry.Instance.GameSetting.LightDarkness = value;
        Entry.Instance.gameSettingObject.SaveData();
        Debug.Log("Set new darkness value and saved setting");
    }

    [RegisterCommand()]
    private static void ToggleTrailerMode(CommandArg[] args)
    {
        Entry.Instance.GameSetting.trailerMode = !Entry.Instance.GameSetting.trailerMode;
        Entry.Instance.gameSettingObject.SaveData();

        ///
        Debug.LogFormat("trailerMode = {0}", Entry.Instance.GameSetting.trailerMode);
        Debug.Log("Saved setting!");
    }

    [RegisterCommand()]
    private static void ToggleHideVersionInfo(CommandArg[] args)
    {
        Entry.Instance.GameSetting.hideVersionInfo = !Entry.Instance.GameSetting.hideVersionInfo;
        Entry.Instance.gameSettingObject.SaveData();

        ///
        Debug.LogFormat("hideVersionInfo = {0}", Entry.Instance.GameSetting.hideVersionInfo);
        Debug.Log("Saved setting!");
    }

    [RegisterCommand(Help = "ResetTutorial")]
    private static void ResetTutorial(CommandArg[] args)
    {
        Entry.Instance.GameSetting.ResetTutorial();

        ///
        Debug.Log("Reset tutorial");
    }

    [RegisterCommand(Help = "")]
    private static void DisableSave(CommandArg[] args)
    {
        Entry.Instance.playerDataSaver.AddUnsavableLock(lockObject);
    }

    [RegisterCommand(Help = "")]
    private static void EnableSave(CommandArg[] args)
    {
        Entry.Instance.playerDataSaver.RemoveUnsavableLock(lockObject);
        Debug.LogFormat("Is savable: {0}", Entry.Instance.playerDataSaver.IsSavable);
    }

    [RegisterCommand(Help = "")]
    private static void ToggleRevealAllItems(CommandArg[] args)
    {
        ///
        Entry.Instance.GameSetting.revealAllItemInDataBase = !Entry.Instance.GameSetting.revealAllItemInDataBase;
        Entry.Instance.gameSettingObject.SaveData();

        ///
        Debug.LogFormat("Done! revealAllItemInDataBase = {0}", Entry.Instance.GameSetting.revealAllItemInDataBase);
        Debug.Log("Saved setting!");
    }

    [RegisterCommand()]
    private static void ToggleShowInputDevice(CommandArg[] args)
    {
        ///
        SimplifiedDeviceRecognition.LogDetection = !SimplifiedDeviceRecognition.LogDetection;

        ///
        Debug.LogFormat("Logging Device: {0}", SimplifiedDeviceRecognition.LogDetection);
    }
    #endregion Settings

    #region Common
    [RegisterCommand(Help = "")]
    private static void ToggleUnlockedPremium(CommandArg[] args)
    {
        throw new System.NotImplementedException();
    }

    [RegisterCommand(Help = "string sceneName")]
    private static void LoadScene(CommandArg[] args)
    {
        string sceneName = args[0].String;

        SceneManager.LoadScene(sceneName);
    }

    [RegisterCommand(Help = "")]
    private static void ReloadCurrentScene(CommandArg[] args)
    {
        var sceneName = VisualSceneLoader.GetGameSceneName(Entry.ActiveGameScene);
        SceneManager.LoadScene(sceneName);
    }

    [RegisterCommand(Help = "")]
    private static void ScreenshotMode(CommandArg[] args)
    {
        CommonCheatLib.AddScreenshotModeLock(lockObject);
    }

    [RegisterCommand(Help = "")]
    private static void UnscreenshotMode(CommandArg[] args)
    {
        CommonCheatLib.RemoveScreenshotModeLock(lockObject);
    }

    [RegisterCommand()]
    private static void SaveDataNow(CommandArg[] args)
    {
        Entry.Instance.playerDataSaver.SaveNow();
    }

    [RegisterCommand(Help = "string context", Name = "c")]
    private static void Cheat(CommandArg[] args)
    {
        ///
        string context = null;

        ///
        if (args.Length > 0)
        {
            context = args[0].String;
        }

        ///
        CommonCheatLib.EmitCheatSignal(context);
    }

    [RegisterCommand(Help = "bool runInBackground")]
    private static void SetRunInBackground(CommandArg[] args)
    {
        ///
        bool runInBackground = args[0].Bool;

        ///
        Application.runInBackground = runInBackground;
    }

    [RegisterCommand()]
    private static void UnloadApplication(CommandArg[] args)
    {
        ///
        Application.Unload();
    }

    [RegisterCommand()]
    private static void MuteBackgroundMusic(CommandArg[] args)
    {
        Entry.Instance.audioManager.MusicVolume = 0;
    }

    [RegisterCommand()]
    private static void SlowdownGameplay(CommandArg[] args)
    {
        Entry.Instance.timeScaleManager.AddControl(gameplaySlowDownController);
    }

    [RegisterCommand()]
    private static void UnslowdownGameplay(CommandArg[] args)
    {
        Entry.Instance.timeScaleManager.RemoveControl(gameplaySlowDownController);
    }

    [RegisterCommand()]
    private static void ToggleScientificBigNumber(CommandArg[] args)
    {
        BigNumberString.ScientificMode = !BigNumberString.ScientificMode;
    }
    #endregion Common

    #region Steam
    [RegisterCommand()]
    private static void ResetAllSteamStats(CommandArg[] args)
    {
#if !DISABLESTEAMWORKS
        ///
        bool achievementToo = false;

        ///
        if (args.Length > 0)
        {
            achievementToo = args[0].Bool;
        }

        ///
        if (Steamworks.SteamUserStats.ResetAllStats(achievementToo))
        {
            ///
            Debug.Log("Reset: success");

            ///
            if (Steamworks.SteamUserStats.StoreStats())
            {
                Debug.Log("Store: success");
            }
            else
            {
                ///
                Debug.LogError("Store: failure");
            }
        }
        else
        {
            ///
            Debug.LogError("Reset: failure");
        }
#endif

    }


    [RegisterCommand()]
    private static void DisableSteamStats(CommandArg[] args)
    {
        ///
        Entry.Instance.gameSettingObject.Data.enabledSteamStats = false;
        Entry.Instance.gameSettingObject.SaveData();

        ///
        Debug.Log("Saved Disabled SteamStats to GameSetting");
    }

    [RegisterCommand()]
    private static void EnableSteamStats(CommandArg[] args)
    {
        ///
        Entry.Instance.gameSettingObject.Data.enabledSteamStats = true;
        Entry.Instance.gameSettingObject.SaveData();

        ///
        Debug.Log("Enabled SteamStats.");
    }
    #endregion Steam

    #region Localization

    //[RegisterCommand()]
    //private static void ToggleLocalizationDebug(CommandArg[] args)
    //{
    //    ///
    //    LocalizationManager.DebugFlag = !LocalizationManager.DebugFlag;

    //    ///
    //    Debug.LogFormat("DebugFlag = {0}", LocalizationManager.DebugFlag);
    //}

    [RegisterCommand(Help = "string lang")]
    private static void SetLanguage(CommandArg[] args)
    {
        ///
        string lang = args[0].String;
        string code = args.Length >= 2 ? args[1].String : null;

        ///
        LocalizationManager.SetLanguageAndCode(lang, code);
    }

    [RegisterCommand()]
    private static void CurrentLanaguage(CommandArg[] args)
    {
        Debug.Log(LocalizationManager.CurrentLanguage);
    }
    #endregion Localization

    #region Run
    [RegisterCommand(Help = "int sourceIndex, int targetIndex, bool copySkills")]
    private static void CopySlot(CommandArg[] args)
    {
        int sourceIndex = args[0].Int;
        int targetIndex = args[1].Int;
        bool copySkills = args[2].Bool;

        var sourceData = Entry.Instance.runDataManager.GetRunDataObject(sourceIndex).Data;
        if (!sourceData.InitedRun)
        {
            Debug.LogError("source data not inited");
            return;
        }
        var targetDataObject = Entry.Instance.runDataManager.GetRunDataObject(targetIndex);
        var targetData = targetDataObject.Data;
        if (targetData.InitedRun)
        {
            Debug.LogError("source data inited!");
            return;
        }

        ///
        targetData.CopyFrom(sourceData, targetIndex, copySkills);
        targetDataObject.SaveData();

        ///
        Debug.Log("Copied! Refresh the slot selection screen!");
    }

    [RegisterCommand(Help = "")]
    private static void EnableDebugTexts(CommandArg[] args)
    {
        CommonCheatLib.DisplayDebugTexts = true;
    }

    [RegisterCommand(Help = "string currency, double amount")]
    private static void AddCurrency(CommandArg[] args)
    {
        ///
        var currencyString = args[0].String.ToUpper();

        ///
        var currency = Enum.Parse<Currency>(currencyString);
        var amount = args[1].Double;

        ///
        RunEntry.Instance.RunData.AddCurrency(currency, amount);
    }

    [RegisterCommand(Help = "string currency, double amount")]
    private static void Spend(CommandArg[] args)
    {
        var currency = Enum.Parse<Currency>(args[0].String.ToUpper());
        var amount = args[1].Double;

        ///
        var success = RunEntry.Instance.RunData.SpendCurrency(currency, amount);
        Debug.Log($"Spent {amount} {currency}, success: {success}");
    }

    [RegisterCommand(Help = "string currency")]
    private static void SpendAll(CommandArg[] args)
    {
        var currency = Enum.Parse<Currency>(args[0].String.ToUpper());
        var amount = RunEntry.Instance.RunData.GetCurrencyValue(currency);

        ///
        var success = RunEntry.Instance.RunData.SpendCurrency(currency, amount);
        if (!success)
        {
            Debug.LogError("Failed to spend all currency! This should never happen!");
        }
        else
        {
            Debug.LogFormat("Spent all {0}!", currency);
        }
    }
    #endregion Run

    #region Combat  

    [RegisterCommand(Help = "")]
    private static void Reset(CommandArg[] args)
    {
        RunEntry.Instance.runStateManager.StartPrestige(null);
    }
    #endregion Combat

    //[RegisterCommand(Help = "")]
    //private static void Tmp(CommandArg[] args)
    //{

    //}
}

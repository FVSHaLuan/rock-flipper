using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMusicToggle : SettingToggle
{
    protected override bool GetValue()
    {
        return Entry.Instance.GameSetting.UseDemoMusic;
    }

    protected override void SetValue(bool value)
    {
        Entry.Instance.GameSetting.UseDemoMusic = value;
    }
}

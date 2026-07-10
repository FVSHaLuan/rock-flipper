using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileReporterToggle : SettingToggle
{
    protected override bool GetValue()
    {
        return Entry.Instance.GameSetting.enableReporterOnMobile;
    }

    protected override void SetValue(bool value)
    {
        Entry.Instance.GameSetting.enableReporterOnMobile = value;

        ///
        SetReporterState.Set();
    }
}

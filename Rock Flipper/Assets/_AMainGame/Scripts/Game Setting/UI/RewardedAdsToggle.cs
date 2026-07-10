using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedAdsToggle : SettingToggle
{
    protected override bool GetValue()
    {
        return Entry.Instance.GameSetting.enabledRewardedAds;
    }

    protected override void SetValue(bool value)
    {
        Entry.Instance.GameSetting.enabledRewardedAds = value;
    }
}

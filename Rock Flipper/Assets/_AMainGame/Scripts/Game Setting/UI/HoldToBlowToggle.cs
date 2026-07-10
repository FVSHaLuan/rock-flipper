using UnityEngine;

namespace Agame.UI.GameSettings
{
    public class HoldToBlowToggle : SettingToggle
    {
        protected override bool GetValue()
        {
            return Entry.Instance.GameSetting.holdToBlow;
        }

        protected override void SetValue(bool value)
        {
            Entry.Instance.GameSetting.holdToBlow = value;
        }
    }

}
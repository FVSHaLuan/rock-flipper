using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI.GameSettings
{
    public class PauseWhenLostFocusToggle : SettingToggle
    {
        protected override bool GetValue()
        {
            return Entry.Instance.GameSetting.pauseWhenLostFocus;
        }

        protected override void SetValue(bool value)
        {
            Entry.Instance.GameSetting.pauseWhenLostFocus = value;
        }
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI.GameSettings
{
    public class ShowDemoLockedItemSettingToggle : SettingToggle
    {
        protected override bool GetValue()
        {
            return Entry.Instance.GameSetting.showDemoLockedItem;
        }

        protected override void SetValue(bool value)
        {
            Entry.Instance.GameSetting.showDemoLockedItem = value;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI.GameSettings
{
    public class DisableLightingToggle : SettingToggle
    {
        protected override bool GetValue()
        {
            return Entry.Instance.GameSetting.DisableLighting;
        }

        protected override void SetValue(bool value)
        {
            Entry.Instance.GameSetting.DisableLighting = value;
        }
    }
}

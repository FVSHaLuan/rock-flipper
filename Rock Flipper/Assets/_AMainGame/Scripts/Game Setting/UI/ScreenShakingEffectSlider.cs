using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI.GameSettings
{
    public class ScreenShakingEffectSlider : SettingSlider
    {
        protected override float GetValue()
        {
            return gameSetting.screenShakingEffect;
        }

        protected override void SetValue(float value)
        {
            gameSetting.screenShakingEffect = value;
        }
    }

}
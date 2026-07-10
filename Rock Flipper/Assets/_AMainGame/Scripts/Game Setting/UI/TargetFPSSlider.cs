using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI.GameSettings
{
    public class TargetFPSSlider : SettingSlider
    {
        [SerializeField]
        private UnifiedText optionNameText;

        [Space]
        [SerializeField]
        private List<int> fpsOptions;

        protected override float GetValue()
        {
            var targetFPS = gameSetting.TargetFPS;
            int foundFPS = 0;

            ///            
            for (int i = 0; i < fpsOptions.Count; i++)
            {
                foundFPS = fpsOptions[i];

                ///
                if (foundFPS >= targetFPS
                    || i == fpsOptions.Count - 1)
                {
                    ///
                    return i;
                }
            }

            return 0;
        }

        protected override void SetValue(float value)
        {
            gameSetting.TargetFPS = fpsOptions[(int)value];
        }

        protected override void DisplayValue(float value)
        {
            optionNameText.Text = fpsOptions[(int)value].ToStringCached();
        }
    }

}
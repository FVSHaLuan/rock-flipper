using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.UI.GameSettings
{
    public class InputDeviceOptionSlider : SettingSlider
    {
        [SerializeField]
        private UnifiedText optionNameText;

        [Space]
        [SerializeField]
        private List<LocalizedString> optionNames;

        protected override void OnEnable()
        {
            ///
            base.OnEnable();

            ///
            GetComponent<Slider>().interactable = !GameSetting.ForceMouseAndKeyboard;
        }

        protected override float GetValue()
        {
            return (int)entry.GameSetting.InputDetectionMode;
        }

        protected override void SetValue(float value)
        {
            entry.GameSetting.InputDetectionMode = (InputDetectionMode)value;
        }

        protected override void DisplayValue(float value)
        {
            optionNameText.Text = optionNames[(int)value];
        }
    }
}

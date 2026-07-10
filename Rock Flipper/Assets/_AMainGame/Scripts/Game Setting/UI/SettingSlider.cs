using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.UI.GameSettings
{
    [RequireComponent(typeof(Slider))]
    public abstract class SettingSlider : ExtendedMonoBehaviour
    {
        private Slider slider;

        protected override bool Init()
        {
            ///
            slider = GetComponent<Slider>();

            ///
            return base.Init();
        }

        protected abstract float GetValue();
        protected abstract void SetValue(float value);

        protected virtual void DisplayValue(float value) { }

        protected void OnDisable()
        {
            ///
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        protected virtual void OnEnable()
        {
            slider.value = GetValue();
            DisplayValue(slider.value);

            ///
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float newValue)
        {
            SetValue(newValue);
            DisplayValue(newValue);
        }
    }

}
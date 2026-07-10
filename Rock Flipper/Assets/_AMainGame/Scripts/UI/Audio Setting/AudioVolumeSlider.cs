using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider)), DisallowMultipleComponent]
public abstract class AudioVolumeSlider : ExtendedMonoBehaviour
{
    private Slider slider;

    protected abstract float Volume { get; set; }

    protected override void ExtendedAwake()
    {
        slider = GetComponent<Slider>();
    }

    public void OnEnable()
    {
        ///
        slider.normalizedValue = Volume;

        ///
        slider.onValueChanged.AddListener(OnSliderChangedValue);
    }

    public void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnSliderChangedValue);
    }

    private void OnSliderChangedValue(float value)
    {
        Volume = slider.normalizedValue;
    }
}

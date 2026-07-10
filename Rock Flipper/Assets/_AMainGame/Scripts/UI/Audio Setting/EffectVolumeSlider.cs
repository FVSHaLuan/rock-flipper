using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectVolumeSlider : AudioVolumeSlider
{
    protected override float Volume
    {
        get => entry.audioManager.EffectVolume;
        set => entry.audioManager.EffectVolume = value;
    }
}

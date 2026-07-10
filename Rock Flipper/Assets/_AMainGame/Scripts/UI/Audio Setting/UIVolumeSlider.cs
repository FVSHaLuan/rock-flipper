using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVolumeSlider : AudioVolumeSlider
{
    protected override float Volume
    {
        get => entry.audioManager.UIVolume;
        set => entry.audioManager.UIVolume = value;
    }
}

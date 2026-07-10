using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeSlider : AudioVolumeSlider
{
    protected override float Volume
    {
        get => entry.audioManager.MasterVolume;
        set => entry.audioManager.MasterVolume = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeSlider : AudioVolumeSlider
{
    protected override float Volume
    {
        get => entry.audioManager.MusicVolume;
        set => entry.audioManager.MusicVolume = value;
    }
}

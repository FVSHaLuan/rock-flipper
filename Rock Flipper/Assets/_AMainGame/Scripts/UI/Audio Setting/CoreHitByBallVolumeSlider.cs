using UnityEngine;

public class CoreHitByBallVolumeSlider : AudioVolumeSlider
{
    protected override float Volume
    {
        get => gameSetting.coreHitSoundEffectVolume;
        set => gameSetting.coreHitSoundEffectVolume = value;
    }
}

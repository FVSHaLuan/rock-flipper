using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundPlayer : GameAudioPlayer
{
    [SerializeField]
    private UISound uiSound = UISound.Press;

    public override void Play()
    {
        switch (uiSound)
        {
            case UISound.Press:
                entry.uiSoundManager.PlayPressSound();
                break;
            case UISound.Navigation:
                entry.uiSoundManager.PlayNavigationSound();
                break;
            case UISound.Hold:
                throw new System.ArgumentException();
            case UISound.DisabledNavigation:
                entry.uiSoundManager.PlayDisabledNavigationSound();
                break;
            case UISound.ItemUnlocked:
                entry.uiSoundManager.PlayItemUnlockedSound();
                break;
            case UISound.ItemUpgraded:
                entry.uiSoundManager.PlayItemUpgradedSound();
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    public void PlayItemUnlockedSound()
    {
        entry.uiSoundManager.PlayItemUnlockedSound();
    }

    public void PlayItemUpgradedSound()
    {
        entry.uiSoundManager.PlayItemUpgradedSound();
    }
}

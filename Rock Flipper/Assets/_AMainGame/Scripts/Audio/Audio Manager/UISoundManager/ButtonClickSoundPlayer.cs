using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSoundPlayer : ExtendedMonoBehaviour
{
    protected void Start()
    {
        GetComponent<Button>()?.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        PlaySoundAndParticleEffect();
    }

    public void PlayClickSound()
    {
        entry.uiSoundManager.PlayPressSound();
    }

    public void PlaySoundAndParticleEffect()
    {
        entry.uiSoundManager.PlayPressSound();
        entry.clickParticleManager.Play();
    }
}

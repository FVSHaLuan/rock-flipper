using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class UISoundManager : ExtendedMonoBehaviour
{
    [SerializeField]
    private GameAudioPlayer pressSoundPlayer;
    [SerializeField]
    private GameAudioPlayer navigationSoundPlayer;
    [SerializeField]
    private GameAudioPlayer disabledNavigationSoundPlayer;
    [SerializeField]
    private UIHoldSoundPlayer uiHoldSoundPlayerPrototype;
    [SerializeField]
    private GameAudioPlayer itemUnlockedSoundPlayer;
    [SerializeField]
    private GameAudioPlayer itemUpgradedSoundPlayer;

    [System.Serializable]
    public class UISoundSerializableDictionary : SerializableDictionary<UISound, AudioClip> { public UISoundSerializableDictionary() { } protected UISoundSerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { } };

    protected override bool Init()
    {
        ///
        uiHoldSoundPlayerPrototype.gameObject.SetActive(false);
        generalPool.TryPushPrototype(uiHoldSoundPlayerPrototype);

        ///
        return base.Init();
    }

    public void PlayPressSound()
    {
        pressSoundPlayer.Play();
    }

    public void PlayNavigationSound()
    {
        navigationSoundPlayer.Play();
    }

    public void PlayDisabledNavigationSound()
    {
        disabledNavigationSoundPlayer.Play();
    }

    public void PlayItemUpgradedSound()
    {
        itemUpgradedSoundPlayer.Play();
    }

    public void PlayItemUnlockedSound()
    {
        itemUnlockedSoundPlayer.Play();
    }

    public UIHoldSoundPlayer TakeUIHoldSoundPlayerInstance()
    {
        ///
        TryInit();

        ///
        return generalPool.TakeInstance(uiHoldSoundPlayerPrototype.PrototypeId, this) as UIHoldSoundPlayer;
    }
}

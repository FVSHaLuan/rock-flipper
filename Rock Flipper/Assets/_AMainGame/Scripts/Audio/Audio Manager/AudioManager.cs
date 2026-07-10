using SubjectNerd.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ExtendedMonoBehaviour
{
    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string EffectVolumeKey = "EffectVolume";
    private const string UIVolumeKey = "UIVolume";

    public event Action OnMasterVolumeChanged;

    [Header("Channel")]
    [SerializeField]
    private AudioChannel musicChannel;
    [SerializeField]
    private AudioChannel uiMusicChannel;
    [SerializeField]
    private AudioChannel uiChannel;
    [SerializeField]
    private AllEffectChannel allEffectChannel;
    [SerializeField]
    private AudioChannel effectChannel;
    [SerializeField]
    private AudioChannel specialEffectChannel;
    [SerializeField]
    private AudioChannel timeWarpChannel;
    [SerializeField]
    private AudioChannel quickDeathChannel;
    [SerializeField]
    private AudioChannel tapChannel;

    [Header("Default volumes")]
    [SerializeField, Range(0.3f, 1)]
    private float defaultMasterVolume = 1.0f;
    [SerializeField, Range(0.3f, 1)]
    private float defaultMusicVolume = 0f;
    [SerializeField, Range(0.3f, 1)]
    private float defaultEffectVolume = 0.8f;
    [SerializeField, Range(0.3f, 1)]
    private float defaultUIVolume = 0.8f;

    [Space]
    [SerializeField, Range(0, 1), Reorderable]
    private List<float> staticGroupVolumes;

    private float masterVolume = -1;
    private List<AudioChannel> allEffectSubChannels;
    private List<GameAudioChannel> effectSubChannelsOrder = new List<GameAudioChannel>() {
        GameAudioChannel.Effect,
        GameAudioChannel.SpecialEffect,
        GameAudioChannel.TimeWarp,
        GameAudioChannel.QuickDeath,
        GameAudioChannel.Tap,
    };

    public float DefaultMusicVolume => defaultMusicVolume;

    public AudioChannelBase MusicChannel => musicChannel;
    public AudioChannelBase EffectChannel => effectChannel;
    public AudioChannelBase UIChannel => uiChannel;

    public List<AudioChannel> AllEffectSubChannels
    {
        get
        {
            ///
            TryInit();

            ///
            return allEffectSubChannels;
        }
    }

    public List<GameAudioChannel> EffectSubChannelsOrder
    {
        get
        {
            ///
            TryInit();

            ///
            return effectSubChannelsOrder;
        }
    }

    public float MasterVolume
    {
        get
        {
            ///
            if (masterVolume < 0)
            {
                masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, defaultMasterVolume);
            }

            ///
            return masterVolume;
        }

        set
        {
            ///
            TryInit();

            ///
            masterVolume = value;

            ///
            PlayerPrefs.SetFloat(MasterVolumeKey, value);

            ///
            OnMasterVolumeChanged?.Invoke();
        }
    }

    public float MusicVolume
    {
        get
        {
            ///
            TryInit();

            ///
            return musicChannel.MainVolume;
        }

        set
        {
            ///
            TryInit();

            ///
            musicChannel.MainVolume = value;
            uiMusicChannel.MainVolume = value;
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }
    }

    public float EffectVolume
    {
        get
        {
            ///
            TryInit();

            ///
            return effectChannel.MainVolume;
        }

        set
        {
            ///
            TryInit();

            ///
            SetMainVolumeForAllEffectSubChannels(value);

            ///
            PlayerPrefs.SetFloat(EffectVolumeKey, value);
        }
    }

    public float UIVolume
    {
        get
        {
            ///
            TryInit();

            ///
            return uiChannel.MainVolume;
        }

        set
        {
            ///
            TryInit();

            ///
            uiChannel.MainVolume = value;
            PlayerPrefs.SetFloat(UIVolumeKey, value);
        }
    }

    protected override bool Init()
    {
        ///        
        uiMusicChannel.MainVolume = musicChannel.MainVolume = PlayerPrefs.GetFloat(MusicVolumeKey, defaultMusicVolume);
        uiChannel.MainVolume = PlayerPrefs.GetFloat(UIVolumeKey, defaultUIVolume);

        ///
        BuildEffectSubChannelLists();
        SetMainVolumeForAllEffectSubChannels(PlayerPrefs.GetFloat(EffectVolumeKey, defaultEffectVolume));

        ///
        return base.Init();
    }

    private void BuildEffectSubChannelLists()
    {
        ///
        allEffectSubChannels = new List<AudioChannel>();

        ///
        foreach (var item in effectSubChannelsOrder)
        {
            var channel = GetAudioChannel(item) as AudioChannel;
            allEffectSubChannels.Add(channel);
        }
    }

    private void SetMainVolumeForAllEffectSubChannels(float volume)
    {
        foreach (var item in allEffectSubChannels)
        {
            item.MainVolume = volume;
        }
    }

    public AudioChannelBase GetAudioChannel(GameAudioChannel gameAudioChannel)
    {
        switch (gameAudioChannel)
        {
            case GameAudioChannel.Music:
                return musicChannel;
            case GameAudioChannel.Effect:
                return effectChannel;
            case GameAudioChannel.SpecialEffect:
                return specialEffectChannel;
            case GameAudioChannel.AllEffect:
                return allEffectChannel;
            case GameAudioChannel.UI:
                return uiChannel;
            case GameAudioChannel.TimeWarp:
                return timeWarpChannel;
            case GameAudioChannel.QuickDeath:
                return quickDeathChannel;
            case GameAudioChannel.Tap:
                return tapChannel;
            case GameAudioChannel.UIMusic:
                return uiMusicChannel;
            default:
                throw new System.NotImplementedException();
        }
    }

    public float GetVolumeFor(StaticVolumeGroup staticVolumeGroup)
    {
        return staticGroupVolumes[(int)staticVolumeGroup];
    }
}

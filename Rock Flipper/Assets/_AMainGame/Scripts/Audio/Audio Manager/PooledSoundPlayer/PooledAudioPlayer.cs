using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD;
using UnityEngine.Audio;

public class PooledAudioPlayer : GameAudioPlayer
{
    [SerializeField]
    private AudioClip audioClip;
    public GameAudioChannel gameAudioChannel = GameAudioChannel.Effect;
    public StaticVolumeGroup staticVolumeGroup = StaticVolumeGroup.Normal;
    [Range(0, 1)]
    public float selfVolume = 1;
    [SerializeField, Range(0, 256)]
    private int priority = 128;
    public AudioMixerGroup mixerGroup;

    [Space]
    [SerializeField]
    private bool playOnEnable;

    [Space]
    [SerializeField]
    private float minPlayTimeInterval = 0;

    [Header("Trim")]
    [SerializeField]
    private float startTrimmingTime = -1;
    [SerializeField]
    private float endTrimmingTime = -1;

    [Space]
    [SerializeField]
    private List<AudioClip> randomAudioClips;

    public AudioClip AudioClip { get; private set; }
    public int Priority { get => priority; set => priority = value; }
    public float EndTrimmingTime { get => endTrimmingTime; set => endTrimmingTime = value; }
    public float StartTrimmingTime { get => startTrimmingTime; set => startTrimmingTime = value; }

    private PooledGameAudioController currentAudioController;
    private int currentPlayId;
    private float lastPlayTimeStamp = -999f;

    protected void OnEnable()
    {
        if (playOnEnable)
        {
            Play();
        }
    }

    [ContextMenu("Play")]
    public override void Play()
    {
        ///
        if ((Time.time - lastPlayTimeStamp) < minPlayTimeInterval)
        {
            return;
        }

        ///
        lastPlayTimeStamp = Time.time;

        ///
        if (randomAudioClips == null || randomAudioClips.Count == 0)
        {
            AudioClip = audioClip;
        }
        else
        {
            ///
            AudioClip = randomAudioClips.GetRandomItem();

            ///
#if UNITY_EDITOR
            if (audioClip != null)
            {
                Debug.LogWarning("audioClip never be used");
            }
#endif
        }

        ///
        currentAudioController = entry.pooledAudioManager.PlayAudio(this);
        currentPlayId = currentAudioController.PlayId;
    }

    public void TryStop()
    {
        ///
        if (currentAudioController == null)
        {
            return;
        }

        ///
        if (!currentAudioController.IsInPool && currentAudioController.PlayId == currentPlayId)
        {
            currentAudioController.StopImmediately();
        }
    }

    public void TryStopThenPlay()
    {
        TryStop();
        Play();
    }

#if UNITY_EDITOR
    protected void Reset()
    {
        ///
        if (GetComponent<RectTransform>() != null)
        {
            gameAudioChannel = GameAudioChannel.UI;
        }
    }
#endif
}

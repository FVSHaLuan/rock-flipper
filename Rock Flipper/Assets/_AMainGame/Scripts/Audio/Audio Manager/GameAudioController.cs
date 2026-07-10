using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class GameAudioController : GameAudioControllerBase
{
    private AudioSource audioSource;

    protected AudioSource AudioSource
    {
        get
        {
            ///
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            ///
            return audioSource;
        }
    }

    protected override float FinalVolume { get => AudioSource.volume; set => AudioSource.volume = value; }
    protected override float FinalPitch { get => AudioSource.pitch; set => AudioSource.pitch = Mathf.Clamp(value, -3, 3); }
    protected override float Length => AudioSource.clip != null ? AudioSource.clip.length : 0;

    protected override float CurrentTime { get => AudioSource.time; set => AudioSource.time = value; }

    protected override bool IsPlaying => AudioSource.isPlaying;

    protected override bool IsLooping => AudioSource.loop;

    protected AudioMixerGroup MixerGroup
    {
        get => AudioSource.outputAudioMixerGroup;
        set => audioSource.outputAudioMixerGroup = value;
    }

    protected override void PlayImmediately()
    {
        AudioSource.Play();
    }

    protected override void _StopImmediately()
    {
        AudioSource.Stop();
    }

#if UNITY_EDITOR
    public override void Reset()
    {
        ///
        base.Reset();

        ///
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().loop = false;
    }
#endif
}

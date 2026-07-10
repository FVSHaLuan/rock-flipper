using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public abstract class GameAudioControllerBase : GameAudioPlayer
{
    [SerializeField, ReadOnly, Range(0, 1)]
    private float editor_audioProgress;

    [Header("GameAudioController")]
    [SerializeField]
    protected GameAudioChannel gameAudioChannel = GameAudioChannel.Effect;
    [SerializeField]
    protected StaticVolumeGroup staticVolumeGroup = StaticVolumeGroup.Normal;
    [SerializeField, Range(0, 1)]
    protected float selfVolume = 1;
    [SerializeField, Range(0, 1)]
    private float controlVolume = 1;
    [SerializeField, Range(0, 3)]
    private float controlPitch = 1;
    [SerializeField]
    private bool alwaysMultiplyPitchByTimeScale = false;
    [SerializeField]
    private TimeScaleMode timeScaleMode = TimeScaleMode.ScaledTime;

    [Header("GameAudioController - fading")]
    [SerializeField]
    private float startFadingDuration;
    [SerializeField]
    private float endFadingDuration;
    [SerializeField]
    private bool autoPlayUsingFading = false;

    [Header("Trim")]
    [SerializeField]
    private float startTrimmingTime = -1;
    [SerializeField]
    private float endTrimmingTime = -1;

    private AudioChannelBase audioChannel;

    private float staticGroupVolume;

    private float fadingVolume = 1;

    public float SelfVolume
    {
        get => selfVolume;
    }

    public float ControlVolume
    {
        get => controlVolume;

        set
        {
            ///
            TryInit();

            ///
            controlVolume = Mathf.Clamp01(value);

            ///
            UpdateAudioSourceVolume();
        }
    }

    public float ControlPitch
    {
        get => controlPitch;

        set
        {
            ///
            TryInit();

            ///
            controlPitch = value;
        }
    }

    public float EndTrimmingTime { get => endTrimmingTime; set => endTrimmingTime = value; }
    public float StartTrimmingTime { get => startTrimmingTime; set => startTrimmingTime = value; }

    protected abstract float FinalVolume { get; set; }
    protected abstract float FinalPitch { get; set; }
    protected abstract float Length { get; }
    protected abstract float CurrentTime { get; set; }
    protected abstract bool IsLooping { get; }
    protected abstract bool IsPlaying { get; }

    protected abstract void PlayImmediately();
    protected abstract void _StopImmediately();

    protected override bool Init()
    {
        ///
        UpdateStaticParameters();

        ///
        return base.Init();
    }

    protected virtual void OnEnable()
    {
        ///
        UpdateAudioSourceVolume();

        ///
        audioChannel.OnEffectiveVolumeChanged += AudioChannel_OnEffectiveVolumeChanged;
    }

    protected virtual void OnDisable()
    {
        ///
        fadingVolume = 1;

        ///
        audioChannel.OnEffectiveVolumeChanged -= AudioChannel_OnEffectiveVolumeChanged;
    }

    private void AudioChannel_OnEffectiveVolumeChanged()
    {
        UpdateAudioSourceVolume();
    }

    protected void UpdateStaticParameters()
    {
        audioChannel = entry.audioManager.GetAudioChannel(gameAudioChannel);
        staticGroupVolume = entry.audioManager.GetVolumeFor(staticVolumeGroup);
    }

    protected void UpdateAudioSourceVolume()
    {
        FinalVolume = fadingVolume * ControlVolume * SelfVolume * audioChannel.EffectiveVolume * staticGroupVolume;
    }

    protected void UpdateAudioSourcePitch()
    {
        ///
        float effectiveTimeScale;

        ///
        if ((gameAudioChannel == GameAudioChannel.Music || gameAudioChannel == GameAudioChannel.UI || gameAudioChannel == GameAudioChannel.TimeWarp) && !alwaysMultiplyPitchByTimeScale)
        {
            effectiveTimeScale = 1;
        }
        else
        {
            effectiveTimeScale = GetTimeScale(timeScaleMode);
        }

        ///
        FinalPitch = controlPitch * effectiveTimeScale;
    }

    public virtual void Update()
    {
        ///
        UpdateAudioSourcePitch();

        ///
        Trim();

        ///
#if UNITY_EDITOR
        ///
        UpdateAudioProgress();
#endif
    }

    public override void Play()
    {
        if (autoPlayUsingFading)
        {
            PlayUsingFading();
        }
        else
        {
            PlayImmediately();
        }
    }

    [ContextMenu("PlayUsingFading")]
    public void PlayUsingFading()
    {
        StopAllCoroutines();
        CurrentTime = startTrimmingTime >= 0 ? startTrimmingTime : 0;
        PlayImmediately();
        StartCoroutine(Fade(1, startFadingDuration, null));
    }

    [ContextMenu("StopUsingFading")]
    public void StopUsingFading()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(0, endFadingDuration, _StopImmediately));
    }

    public void StopImmediately()
    {
        StopAllCoroutines();
        _StopImmediately();
    }

    private IEnumerator Fade(float targetFadingVolume, float duration, Action callback)
    {
        ///
        if (Mathf.Approximately(duration, 0))
        {
            fadingVolume = targetFadingVolume;
            UpdateAudioSourceVolume();

            ///
            callback?.Invoke();

            ///
            yield break;
        }

        ///
        var fadingSpeed = 1.0f / duration;

        ///
        while (!Mathf.Approximately(fadingVolume, targetFadingVolume))
        {
            fadingVolume = Mathf.MoveTowards(fadingVolume, targetFadingVolume, fadingSpeed * Time.deltaTime);

            ///
            UpdateAudioSourceVolume();

            ///
            yield return null;
        }

        ///
        fadingVolume = targetFadingVolume;
        UpdateAudioSourceVolume();

        ///
        callback?.Invoke();
    }

    protected void Trim()
    {
        ///
        if (!IsPlaying)
        {
            return;
        }

        // End trimming
        if (EndTrimmingTime > 0 && CurrentTime >= EndTrimmingTime)
        {
            if (IsLooping)
            {
                CurrentTime = Mathf.Clamp(StartTrimmingTime, 0, Length);
            }
            else
            {
                _StopImmediately();
            }
        }

        // Start trimming
        if (StartTrimmingTime > 0 && CurrentTime < StartTrimmingTime)
        {
            CurrentTime = Mathf.Clamp(StartTrimmingTime, 0, Length);
        }
    }

#if UNITY_EDITOR

    private void UpdateAudioProgress()
    {
        ///
        if (Mathf.Approximately(Length, 0))
        {
            return;
        }

        ///
        editor_audioProgress = CurrentTime / Length;
    }

    public virtual void Reset()
    {
        ///
        if (GetComponent<RectTransform>() != null)
        {
            gameAudioChannel = GameAudioChannel.UI;
            timeScaleMode = TimeScaleMode.UnscaledTime;
        }
    }
#endif
}
using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChannel : AudioChannelBase
{
    private const float DimVolume = 0.1f;
    private const float ChangeDuration = 0.5f;
    private const float StartupDelayTime = 0.5f;
    private const float StartupFadeInTime = 0.5f;

    public override event System.Action OnEffectiveVolumeChanged;

    [SerializeField]
    private bool fadeInOnStartup = true;

    private float mainVolume = 1;
    private float temporaryVolume = 1;
    private BalancerWithObjects undimBalancer = new BalancerWithObjects();
    private BalancerWithObjects unmutedBalancer = new BalancerWithObjects();
    private float effectiveVolumeBeforeStartupFactor;
    private float effectiveVolume;
    private float startupFactor;
    private bool isUpdatingStartupFactor;

    private float changeSpeed;

    public override float EffectiveVolume => effectiveVolume;

    public float MainVolume
    {
        get => mainVolume;
        set
        {
            ///
            mainVolume = Mathf.Clamp01(value);

            ///
            UpdateChangeSpeed();
            UpdateTemporaryVolume();

            ///
            effectiveVolumeBeforeStartupFactor = temporaryVolume;
            effectiveVolume = effectiveVolumeBeforeStartupFactor * startupFactor;

            ///
            OnEffectiveVolumeChanged?.Invoke();
        }
    }

    protected void OnEnable()
    {
        ///
        StartCoroutine(UpdateStartupFactor());

        ///
        Entry.Instance.audioManager.OnMasterVolumeChanged += AudioManager_OnMasterVolumeChanged;
    }

    protected void OnDisable()
    {
        ///
        Entry.Instance.audioManager.OnMasterVolumeChanged -= AudioManager_OnMasterVolumeChanged;
    }

    public AudioChannel()
    {
        ///
        undimBalancer.OnBalanceChanged += UndimedBalancer_OnBalanceChanged;
        unmutedBalancer.OnBalanceChanged += UnmutedBalancer_OnBalanceChanged;
    }

    private void AudioManager_OnMasterVolumeChanged()
    {
        UpdateTemporaryVolume();
    }

    public override void ClearMute()
    {
        unmutedBalancer.Reset();
    }

    public override void ClearDim()
    {
        undimBalancer.Reset();
    }

    public override void AddMute(object @object)
    {
        unmutedBalancer.AddObject(@object);
    }

    public override void RemoveMute(object @object)
    {
        unmutedBalancer.RemoveObject(@object);
    }

    public override void AddDim(object @object)
    {
        undimBalancer.AddObject(@object);
    }

    public override void RemoveDim(object @object)
    {
        undimBalancer.RemoveObject(@object);
    }

    private void UnmutedBalancer_OnBalanceChanged()
    {
        UpdateTemporaryVolume();
        UpdateEffectiveVolume();
    }

    private void UndimedBalancer_OnBalanceChanged()
    {
        UpdateTemporaryVolume();
        UpdateEffectiveVolume();
    }

    public void LateUpdate()
    {
        UpdateEffectiveVolume();
    }

    [ContextMenu("UpdateTemporaryVolume")]
    private void UpdateTemporaryVolume()
    {
        ///
        if (!unmutedBalancer.IsBalanced)
        {
            temporaryVolume = 0;
            return;
        }

        ///
        if (!undimBalancer.IsBalanced)
        {
            temporaryVolume = Mathf.Min(MainVolume, DimVolume) * Entry.Instance.audioManager.MasterVolume;
            return;
        }

        ///
        temporaryVolume = MainVolume * Entry.Instance.audioManager.MasterVolume;
    }

    private void UpdateChangeSpeed()
    {
        changeSpeed = MainVolume / ChangeDuration;
    }

    [ContextMenu("UpdateEffectiveVolume")]
    private void UpdateEffectiveVolume()
    {
        ///
        bool updateToDate = !isUpdatingStartupFactor
            && Mathf.Approximately(temporaryVolume, effectiveVolumeBeforeStartupFactor);

        ///
        if (updateToDate)
        {
            return;
        }

        ///
        effectiveVolumeBeforeStartupFactor = Mathf.MoveTowards(EffectiveVolume, temporaryVolume, changeSpeed * Time.unscaledDeltaTime);

        ///
        effectiveVolume = effectiveVolumeBeforeStartupFactor * startupFactor;

        ///
        OnEffectiveVolumeChanged?.Invoke();
    }

    private IEnumerator UpdateStartupFactor()
    {
        ///
        isUpdatingStartupFactor = true;

        ///
        if (!fadeInOnStartup)
        {
            ///
            startupFactor = 1;

            ///
            yield return null;
            yield return null;

            ///
            isUpdatingStartupFactor = false;

            ///
            yield break;
        }

        ///
        startupFactor = 0;

        ///
        yield return new WaitForSecondsRealtime(StartupDelayTime);

        ///
        float t = 0;
        while (t < StartupFadeInTime)
        {
            ///
            t += Time.unscaledDeltaTime;
            if (t > StartupFadeInTime)
            {
                t = StartupFadeInTime;
            }

            ///
            startupFactor = Mathf.Lerp(0, 1, t / startupFactor);

            ///
            yield return null;
        }

        ///
        startupFactor = 1;

        ///
        yield return null;

        ///
        isUpdatingStartupFactor = false;
    }
}

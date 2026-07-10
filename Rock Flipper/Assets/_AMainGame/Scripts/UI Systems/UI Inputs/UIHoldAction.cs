using GD;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIHoldAction : UIInputAction
{
    public event Action<float> OnUpdatedHoldProgress;
    public event Action OnPassedHoldThreshold;

    [Header("UIHoldAction")]
    [SerializeField]
    private float holdDuration = 0.5f;
    [SerializeField]
    private bool countFromActionStart;
    [SerializeField]
    private ProgressBar progressBar;
    [SerializeField]
    private bool autoHideProgressBar;

    [Header("UIHoldAction -- Sound")]
    [SerializeField]
    private bool playStandardHoldSound = true;
    [SerializeField]
    private bool playStandardActivatedSound = true;

    [Header("UIHoldAction -- Events")]
    [SerializeField]
    private UnityEvent onPassedHoldThreshold;

    private bool invokedPassedHoldThresholdEvents;

    private float startCountingTime = -1;

    private UIHoldSoundPlayer uiHoldSoundPlayer;

    public ProgressBar ProgressBar
    {
        set
        {
            progressBar = value;
        }
    }

    protected override void Awake()
    {
        ///
        base.Awake();

        ///
        OnActionStarted += UIHoldAction_OnActionStarted;
        OnActionPerformed += UIHoldAction_OnActionPerformed;
        OnActionDisrupted += UIHoldAction_OnActionDisrupted;
        OnActionCanceled += UIHoldAction_OnActionCanceled;
    }

    protected override void OnEnable()
    {
        ///
        base.OnEnable();

        ///
        if (startCountingTime >= 0 && !invokedPassedHoldThresholdEvents)
        {
            ///
            UpdateProgressBar();
        }
        else
        {
            if (progressBar != null)
            {
                progressBar.SetValue(0);
            }
        }
    }

    private void UIHoldAction_OnActionStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ///
        if (countFromActionStart)
        {
            StartHolding();
        }

        ///
        invokedPassedHoldThresholdEvents = false;
    }

    private void UIHoldAction_OnActionCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        DisruptHolding();
    }

    private void UIHoldAction_OnActionDisrupted(bool performed, float durationFromStarted, float durationFromPerformed)
    {
        DisruptHolding();
    }

    private void UIHoldAction_OnActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ///
        if (countFromActionStart)
        {
            return;
        }

        ///
        StartHolding();
    }

    protected override void Update()
    {
        ///
        base.Update();

        ///
        if (startCountingTime >= 0 && !invokedPassedHoldThresholdEvents)
        {
            ///
            UpdateProgressBar();

            ///
            var timePassed = Time.unscaledTime - startCountingTime;
            if (timePassed > holdDuration)
            {
                ///
                onPassedHoldThreshold?.Invoke();
                OnPassedHoldThreshold?.Invoke();
                invokedPassedHoldThresholdEvents = true;

                ///
                progressBar?.SetValue(0);
                OnUpdatedHoldProgress?.Invoke(0);

                ///
                StopHoldingSound(true);
            }
        }
    }

    private void UpdateProgressBar()
    {
        ///
        var timePassed = Time.unscaledTime - startCountingTime;
        var progress = timePassed / holdDuration;

        ///
        var progressValue = Mathf.Clamp01(progress);

        ///
        if (progressBar != null)
        {
            progressBar.SetValue(progressValue);
        }

        ///
        OnUpdatedHoldProgress?.Invoke(progressValue);
    }

    protected override void PlayPressSound()
    {
        // Intended blank
    }

    private void StartHolding()
    {
        ///
        startCountingTime = Time.unscaledTime;

        ///
        if (autoHideProgressBar && progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
        }

        ///
        if (playStandardHoldSound)
        {
            uiHoldSoundPlayer = Entry.Instance.uiSoundManager.TakeUIHoldSoundPlayerInstance();
            uiHoldSoundPlayer.PlayActivatedSound = playStandardActivatedSound;
            uiHoldSoundPlayer.StartHolding();
        }
        else
        {
            uiHoldSoundPlayer = null;
        }
    }

    private void DisruptHolding()
    {
        ///
        progressBar?.SetValue(0);
        OnUpdatedHoldProgress?.Invoke(0);

        ///
        startCountingTime = -1;

        ///
        if (autoHideProgressBar && progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }

        ///
        StopHoldingSound(false);
    }

    private void StopHoldingSound(bool activated)
    {
        if (uiHoldSoundPlayer != null)
        {
            uiHoldSoundPlayer.Release(activated);
        }
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        ///
        base.Reset();

        ///
        playSound = false;
    }
#endif
}

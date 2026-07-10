using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleManager : ExtendedMonoBehaviour
{
    private const float MinFixedTimestep = 0.0001f;
    private const float GameplaySlowTimeScale = 0.2f;

    public event System.Action OnTimeStopped;
    public event System.Action OnTimeResumed;
    public event System.Action OnGamePaused;
    public event System.Action OnGameUnpaused;
    public event System.Action OnGameplayStartedSlowingDown;
    public event System.Action OnGameplayStoppedSlowingDown;

    private float timeScale = 1;

    private List<ITimeScaleControl> controls = new List<ITimeScaleControl>();

    private float maxFixedTimestep;
    private BalancerWithObjects unpausedBalancer = new BalancerWithObjects();
    private BalancerWithObjects unslowdownGameplayBalancer = new BalancerWithObjects();
    private float gameplayUnscaledTime = -1;
    private float gameplayUnscaledTimeAbsolute = -1;
    private int lastUpdatedFrameCount = -1;

    public bool HasTimeStopped { get; private set; } = false;
    public bool IsGameplayBeingPaused => !unpausedBalancer.IsBalanced;
    public bool IsGameplayBeingSlowedDown => !unslowdownGameplayBalancer.IsBalanced;
    public new float GameplayUnscaledTime
    {
        get
        {
            ///
            TryDoPerFrameUpdate();

            ///
            return gameplayUnscaledTime;
        }

        private set
        {
            ///
            gameplayUnscaledTime = value;
        }
    }

    /// <summary>
    /// Ignore gameplay slow effect
    /// </summary>
    public new float GameplayUnscaledDeltaTimeAbsolute
    {
        get
        {
            if (unpausedBalancer.IsBalanced)
            {
                return Time.unscaledDeltaTime;
            }
            else
            {
                return 0;
            }
        }

    }

    public new float GameplayUnscaledDeltaTime
    {
        get
        {
            if (unpausedBalancer.IsBalanced)
            {
                if (unslowdownGameplayBalancer.IsBalanced)
                {
                    return Time.unscaledDeltaTime;
                }
                else
                {
                    return Time.deltaTime;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    public float GameplayFixedUnscaledDeltaTime
    {
        get
        {
            if (unpausedBalancer.IsBalanced)
            {
                if (unslowdownGameplayBalancer.IsBalanced)
                {
                    return Time.fixedUnscaledDeltaTime;
                }
                else
                {
                    return Time.fixedDeltaTime;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    protected override bool Init()
    {
        ///
        maxFixedTimestep = Time.fixedUnscaledDeltaTime;

        ///
        unpausedBalancer.OnBalanceChanged += UnpausedBalancer_OnBalanceChanged;
        unpausedBalancer.OnBalanced += UnpausedBalancer_OnBalanced;
        unpausedBalancer.OnOffBalanced += UnpausedBalancer_OnOffBalanced;
        unslowdownGameplayBalancer.OnBalanceChanged += UnslowdownGameplayBalancer_OnBalanceChanged;
        unslowdownGameplayBalancer.OnBalanced += UnslowdownGameplayBalancer_OnBalanced;
        unslowdownGameplayBalancer.OnOffBalanced += UnslowdownGameplayBalancer_OnOffBalanced;

        ///
        return base.Init();
    }

    protected void LateUpdate()
    {
        TryDoPerFrameUpdate();
    }

    private void UnslowdownGameplayBalancer_OnOffBalanced()
    {
        OnGameplayStartedSlowingDown?.Invoke();
    }

    private void UnslowdownGameplayBalancer_OnBalanced()
    {
        OnGameplayStoppedSlowingDown?.Invoke();
    }

    private void UnslowdownGameplayBalancer_OnBalanceChanged()
    {
        UpdateTimeScale();
    }

    private void UnpausedBalancer_OnOffBalanced()
    {
        OnGamePaused?.Invoke();
    }

    private void UnpausedBalancer_OnBalanced()
    {
        OnGameUnpaused?.Invoke();
    }

    private void UnpausedBalancer_OnBalanceChanged()
    {
        UpdateTimeScale();
    }

    public void AddControl(ITimeScaleControl timeScaleControl)
    {
        ///
        controls.Add(timeScaleControl);

        ///
        timeScaleControl.OnControlValueChanged += TimeScaleControl_OnControlValueChanged;

        ///
        UpdateTimeScale();
    }

    private void TimeScaleControl_OnControlValueChanged()
    {
        UpdateTimeScale();
    }

    public void RemoveControl(ITimeScaleControl timeScaleControl)
    {
        if (controls.Remove(timeScaleControl))
        {
            ///
            timeScaleControl.OnControlValueChanged -= TimeScaleControl_OnControlValueChanged;

            ///
            UpdateTimeScale();
        }
    }

    private void UpdateTimeScale()
    {
        ///
        float newTimeScale = 1;

        ///
        if (unpausedBalancer.IsBalanced)
        {
            ///
            newTimeScale = CalculateTimeScaleFromTheControlList(newTimeScale);

            ///
            if (unslowdownGameplayBalancer.IsBalanced)
            {
                ///
                // Intentionally left blank
            }
            else
            {
                newTimeScale = Mathf.Min(newTimeScale * GameplaySlowTimeScale, GameplaySlowTimeScale);
            }
        }
        else
        {
            newTimeScale = 0;
        }

        ///
        var savedTimeScale = timeScale;

        ///
        timeScale = newTimeScale;
        UpdateTimeScale(timeScale);

        ///
        if (!Mathf.Approximately(savedTimeScale, timeScale))
        {
            if (Mathf.Approximately(timeScale, 0))
            {
                ///
                OnTimeStopped?.Invoke();
            }
            else if (Mathf.Approximately(savedTimeScale, 0))
            {
                ///
                OnTimeResumed?.Invoke();
            }
        }
    }

    private float CalculateTimeScaleFromTheControlList(float newTimeScale)
    {
        for (int i = 0; i < controls.Count; i++)
        {
            ///
            var control = controls[i];

            ///
            switch (control.ControlType)
            {
                case TimeScaleControlType.Override:
                    newTimeScale = control.ControlValue;
                    break;
                case TimeScaleControlType.Multiply:
                    newTimeScale *= control.ControlValue;
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        return newTimeScale;
    }

    private void UpdateTimeScale(float timeScale)
    {
        ///
        Time.timeScale = timeScale;

        ///
        HasTimeStopped = Mathf.Approximately(timeScale, 0);

        ///
        if (!IsGameplayBeingPaused)
        {
            Time.fixedDeltaTime = Mathf.Lerp(MinFixedTimestep, maxFixedTimestep, timeScale);
        }
        else
        {
            Time.fixedDeltaTime = maxFixedTimestep;
        }
    }

    public void AddPauseGame(object @object)
    {
        ///
        unpausedBalancer.AddObject(@object);
    }

    public void RemovePauseGame(object @object)
    {
        ///
        unpausedBalancer.RemoveObject(@object);
    }

    public void AddGameplaySlowdown(object @object)
    {
        unslowdownGameplayBalancer.AddObject(@object);
    }

    public void RemoveGameplaySlowdown(object @object)
    {
        unslowdownGameplayBalancer.RemoveObject(@object);
    }

    public new float GetTimeScale(TimeScaleMode timeScaleMode)
    {
        switch (timeScaleMode)
        {
            case TimeScaleMode.ScaledTime:
                return Time.timeScale;
            case TimeScaleMode.GameplayUnscaledTime:
                if (unpausedBalancer.IsBalanced)
                {
                    if (unslowdownGameplayBalancer.IsBalanced)
                    {
                        return 1;
                    }
                    else
                    {
                        return GameplaySlowTimeScale;
                    }
                }
                else
                {
                    return 0;
                }
            case TimeScaleMode.UnscaledTime:
                return 1;
            case TimeScaleMode.GameplayUnscaledTimeAbsolute:
                if (unpausedBalancer.IsBalanced)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            default:
                throw new System.NotImplementedException();
        }
    }

    public new float GetDeltaTime(TimeScaleMode timeScaleMode)
    {
        switch (timeScaleMode)
        {
            case TimeScaleMode.ScaledTime:
                return Time.deltaTime;
            case TimeScaleMode.GameplayUnscaledTime:
                return GameplayUnscaledDeltaTime;
            case TimeScaleMode.UnscaledTime:
                return Time.unscaledDeltaTime;
            case TimeScaleMode.GameplayUnscaledTimeAbsolute:
                return GameplayUnscaledDeltaTimeAbsolute;
            default:
                throw new System.NotImplementedException();
        }
    }

    public new float GetTime(TimeScaleMode timeScaleMode)
    {
        switch (timeScaleMode)
        {
            case TimeScaleMode.ScaledTime:
                return Time.time;
            case TimeScaleMode.GameplayUnscaledTime:
                return GameplayUnscaledTime;
            case TimeScaleMode.UnscaledTime:
                return Time.unscaledTime;
            case TimeScaleMode.GameplayUnscaledTimeAbsolute:
                return gameplayUnscaledTimeAbsolute;
            default:
                throw new System.NotImplementedException();
        }
    }

    private void TryDoPerFrameUpdate()
    {
        ///
        if (lastUpdatedFrameCount == Time.frameCount)
        {
            return;
        }

        ///
        lastUpdatedFrameCount = Time.frameCount;

        // gameplayUnscaledTime
        if (gameplayUnscaledTime < 0)
        {
            gameplayUnscaledTime = Time.unscaledTime;
        }
        else
        {
            gameplayUnscaledTime += GameplayUnscaledDeltaTime;
        }

        // gameplayUnscaledTimeAbsolute
        if (gameplayUnscaledTimeAbsolute < 0)
        {
            gameplayUnscaledTimeAbsolute = Time.unscaledTime;
        }
        else
        {
            gameplayUnscaledTimeAbsolute += GameplayUnscaledDeltaTimeAbsolute;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class GameplayTimeScaleSetter : ExtendedMonoBehaviour
{
    [SerializeField]
    private TimeScaleMode timeScaleMode = TimeScaleMode.GameplayUnscaledTime;

    private bool useUnscaledTime;

    protected bool UseUnscaledTime => useUnscaledTime;

    protected abstract void Set(bool useUnscaledTime);

    protected void OnEnable()
    {
        useUnscaledTime = ShouldUseUnscaledTime();
        Set(useUnscaledTime);
    }

    protected void Update()
    {
        ///
        if (useUnscaledTime != ShouldUseUnscaledTime())
        {
            useUnscaledTime = !useUnscaledTime;
            Set(useUnscaledTime);
        }
    }

    private bool ShouldUseUnscaledTime()
    {
        ///
        var isPausing = entry.timeScaleManager.IsGameplayBeingPaused;
        var isSlowing = entry.timeScaleManager.IsGameplayBeingSlowedDown;

        ///
        bool useUnscaledTime;

        ///
        switch (timeScaleMode)
        {
            case TimeScaleMode.ScaledTime:
                useUnscaledTime = false;
                break;
            case TimeScaleMode.GameplayUnscaledTime:
                useUnscaledTime = !isPausing && !isSlowing;
                break;
            case TimeScaleMode.UnscaledTime:
                useUnscaledTime = true;
                break;
            case TimeScaleMode.GameplayUnscaledTimeAbsolute:
                useUnscaledTime = !isPausing;
                break;
            default:
                throw new System.NotImplementedException();
        }

        ///
        return useUnscaledTime;
    }
}

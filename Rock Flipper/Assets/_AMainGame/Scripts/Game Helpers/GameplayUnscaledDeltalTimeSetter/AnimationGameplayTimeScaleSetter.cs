using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationGameplayTimeScaleSetter : GameplayTimeScaleSetter
{
    private new Animation animation;

    protected void LateUpdate()
    {
        UpdateSpeed(UseUnscaledTime);
    }

    protected override void Set(bool useUnscaledTime)
    {

    }

    private void UpdateSpeed(bool useUnscaledTime)
    {
        ///
        if (animation == null)
        {
            animation = GetComponent<Animation>();
        }

        ///
        var effectiveTimeScale = useUnscaledTime ? 1 : Time.timeScale;
        var speed = Mathf.Approximately(Time.timeScale, 0) ? 1 : effectiveTimeScale / Time.timeScale;

        ///
        foreach (AnimationState state in animation)
        {
            state.speed = speed;
        }
    }
}

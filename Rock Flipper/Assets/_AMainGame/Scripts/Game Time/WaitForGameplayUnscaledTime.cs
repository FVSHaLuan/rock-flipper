using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForGameplayUnscaledTime : CustomYieldInstruction
{
    private float duration;
    private float waitedTime;
    private int lastFrameCount = -1;

    public override bool keepWaiting
    {
        get
        {
            ///
            if (lastFrameCount != Time.frameCount)
            {
                ///
                lastFrameCount = Time.frameCount;

                ///
                waitedTime += Entry.Instance.timeScaleManager.GameplayUnscaledDeltaTime;
            }

            ///
            return waitedTime < duration;
        }
    }

    public WaitForGameplayUnscaledTime(float duration)
    {
        this.duration = duration;
        waitedTime = 0;
    }
}

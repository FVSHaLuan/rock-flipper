using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForUniversalTime : CustomYieldInstruction
{
    private float duration;
    private float waitedTime;
    private int lastFrameCount = -1;
    private TimeScaleMode timeScaleMode;

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
                waitedTime += Entry.Instance.timeScaleManager.GetDeltaTime(timeScaleMode);
            }

            ///
            return waitedTime < duration;
        }
    }

    public WaitForUniversalTime(float duration, TimeScaleMode timeScaleMode)
    {
        this.duration = duration;
        this.timeScaleMode = timeScaleMode;
        waitedTime = 0;
    }
}

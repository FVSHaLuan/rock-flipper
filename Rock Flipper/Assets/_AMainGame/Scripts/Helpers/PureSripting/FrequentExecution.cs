using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequentExecution
{
    public System.Action Callback { get; set; }

    public float CallbackPerSecond { get; set; }

    public float TimePassed { get; set; }

    public void Update(float deltaTime)
    {
        ///
        TimePassed += deltaTime;

        ///
        var interval = 1.0f / CallbackPerSecond;

        ///
        while (TimePassed >= interval)
        {
            ///
            Callback?.Invoke();

            ///
            TimePassed -= interval;
        }
    }
}

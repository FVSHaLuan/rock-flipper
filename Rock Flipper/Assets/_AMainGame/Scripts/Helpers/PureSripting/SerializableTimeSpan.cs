using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UTimeSpan
{
    [Min(0)]
    public int hours;
    [Min(0)]
    public int minutes;
    [Min(0)]
    public int seconds;
    [Min(0)]
    public int milliSeconds;

    public UTimeSpan(int hours, int minutes, int seconds, int milliSeconds)
    {
        this.hours = hours;
        this.minutes = minutes;
        this.seconds = seconds;
        this.milliSeconds = milliSeconds;
    }

    public TimeSpan TimeSpan => new TimeSpan(0, hours, minutes, seconds, milliSeconds);

    public float TotalSeconds => hours * 3600 + minutes * 60 + seconds;

    public static implicit operator TimeSpan(UTimeSpan uTimeSpan) => uTimeSpan.TimeSpan;
}

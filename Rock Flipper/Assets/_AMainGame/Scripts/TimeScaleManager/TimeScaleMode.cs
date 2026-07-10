using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeScaleMode
{
    /// <summary>
    /// Unity's scaled time
    /// </summary>
    ScaledTime = 0,

    /// <summary>
    /// Unscaled time that takes account for game being paused or slowed
    /// </summary>
    GameplayUnscaledTime = 1,

    /// <summary>
    /// Unity's unscaled time
    /// </summary>
    UnscaledTime = 2,

    /// <summary>
    /// Unscaled time that takes account for game being paused only
    /// </summary>
    GameplayUnscaledTimeAbsolute = 3,
}

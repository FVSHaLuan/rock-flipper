using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedMonoBehaviourWithTime : ExtendedMonoBehaviour
{
    [SerializeField]
    protected TimeScaleMode timeScaleMode = TimeScaleMode.ScaledTime;

    protected float GameplayDeltaTime => GetDeltaTime(timeScaleMode);
    protected float GameplayTime => GetTime(timeScaleMode);

    protected virtual void Reset()
    {
#if UNITY_EDITOR
        if (GetComponent<RectTransform>() != null)
        {
            timeScaleMode = TimeScaleMode.GameplayUnscaledTime;
        }
#endif
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleControl : ExtendedMonoBehaviour, ITimeScaleControl
{
    public event Action OnControlValueChanged;

    [SerializeField]
    private TimeScaleControlType controlType = TimeScaleControlType.Override;
    [SerializeField]
    private float controlValue = 1;

    public TimeScaleControlType ControlType
    {
        get => controlType;
        set
        {
            ///
            controlType = value;

            ///
            OnControlValueChanged?.Invoke();
        }
    }

    public float ControlValue
    {
        get => controlValue;

        set
        {
            ///
            controlValue = value;

            ///
            OnControlValueChanged?.Invoke();
        }
    }

    protected void OnEnable()
    {
        entry.timeScaleManager.AddControl(this);
    }

    protected void OnDisable()
    {
        entry.timeScaleManager.RemoveControl(this);
    }
}

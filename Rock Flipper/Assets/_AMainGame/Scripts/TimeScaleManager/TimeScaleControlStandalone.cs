using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleControlStandalone : ITimeScaleControl
{
    public event Action OnControlValueChanged;

    public TimeScaleControlType ControlType { get; private set; }

    public float ControlValue { get; private set; }

    private TimeScaleControlStandalone()
    {

    }

    public TimeScaleControlStandalone(TimeScaleControlType controlType, float controlValue)
    {
        ControlType = controlType;
        ControlValue = controlValue;
    }
}

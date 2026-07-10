using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeScaleControl
{
    event System.Action OnControlValueChanged;
    TimeScaleControlType ControlType { get; }
    float ControlValue { get; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class AnyKeyEventsStandalone : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onAnyKeyPressedThisFrame;

    private int lastInputFrameCount = -1;
    private bool HasAnyKeyPressedThisFrame => lastInputFrameCount == Time.frameCount;

    protected void OnEnable()
    {
        ///
        InputSystem.onEvent += InputSystem_onEvent;
    }

    protected void OnDisable()
    {
        InputSystem.onEvent -= InputSystem_onEvent;
    }

    private void InputSystem_onEvent(InputEventPtr eventPtr, InputDevice device)
    {
        ///
        if (HasAnyKeyPressedThisFrame)
        {
            return;
        }

        ///
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            return;
        var controls = device.allControls;
        var buttonPressPoint = InputSystem.settings.defaultButtonPressPoint;
        for (var i = 0; i < controls.Count; ++i)
        {
            var control = controls[i] as ButtonControl;
            if (control == null || control.synthetic || control.noisy || control.isPressed)
                continue;
            if (control.ReadValueFromEvent(eventPtr, out var value) && value >= buttonPressPoint)
            {
                ///
                lastInputFrameCount = Time.frameCount;

                ///
                onAnyKeyPressedThisFrame?.Invoke();

                ///
                break;
            }
        }
    }
}

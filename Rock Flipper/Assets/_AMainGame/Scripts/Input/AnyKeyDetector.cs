using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class AnyKeyDetector : ExtendedMonoBehaviour
{
    private event System.Action onAnyKeyPressedThisFrame;

    public event System.Action OnAnyKeyPressedThisFrame
    {
        add
        {
            ///
            onAnyKeyPressedThisFrame += value;

            ///
            if (onAnyKeyPressedThisFrame != null)
            {
                AddEnableLock(this);
            }
        }

        remove
        {
            ///
            onAnyKeyPressedThisFrame -= value;

            ///
            if (onAnyKeyPressedThisFrame == null)
            {
                RemoveEnableLock(this);
            }
        }
    }

    private int lastInputFrameCount = -1;
    private BalancerWithObjects disableBalancer = new BalancerWithObjects();

    public bool HasAnyKeyPressedThisFrame => lastInputFrameCount == Time.frameCount;

    private void AddEnableLock(object @object)
    {
        ///
        disableBalancer.AddObject(@object);

        ///
        enabled = !disableBalancer.IsBalanced;
    }

    private void RemoveEnableLock(object @object)
    {
        ///
        disableBalancer.RemoveObject(@object);

        ///
        enabled = !disableBalancer.IsBalanced;
    }

    protected void OnEnable()
    {
        ///
        if (disableBalancer.IsBalanced)
        {
            enabled = false;
            return;
        }

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

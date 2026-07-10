using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : ExtendedMonoBehaviour
{
    public event Action OnActiveInputDeviceChanged;
    public event Action OnSimplifiedInputDeviceTypeChanged;
    public event Action<InputDevice> OnAnyGamepadDisconnected;

    [SerializeField]
    private InputActionAsset inputActionAsset;

    [Space]
    public InputActionReference inputInfoScrollMouse;
    public InputActionReference inputInfoScrollGamePad;

    private bool forceMouseAndKeyBoard;
    private int activeInputDeviceId = -1;
    private SimplifiedInputDevice activeSimplifiedInputDevice;

    private SimplifiedInputDevice ForcedMouseAndKeyboardSimplifiedInputDevice { get; } = new SimplifiedInputDevice()
    {
        deviceType = SimplifiedInputDeviceType.MouseAndKeyboard,
        gamepadType = SimplifiedGamepadType.None,
        originalDeviceName = "ForcedMouseAndKeyboard",
    };

    private SimplifiedInputDevice ForcedGamepadInputDevice { get; } = new SimplifiedInputDevice()
    {
        deviceType = SimplifiedInputDeviceType.Gamepad,
        gamepadType = SimplifiedGamepadType.Other,
        originalDeviceName = "ForcedGamepad",
    };

    public SimplifiedInputDevice ActiveSimplifiedInputDevice
    {
        get
        {
            ///
            TryInit();

            ///
            if (GameReadOnlySetting.ForcedMouseAndKeyboard)
            {
                return ForcedMouseAndKeyboardSimplifiedInputDevice;
            }

            ///
            var detectionMode = entry.GameSetting.InputDetectionMode;
            if (forceMouseAndKeyBoard)
            {
                detectionMode = InputDetectionMode.MouseAndKeyBoard;
            }

            ///
            switch (detectionMode)
            {
                case InputDetectionMode.Auto:
                    return activeSimplifiedInputDevice;
                case InputDetectionMode.MouseAndKeyBoard:
                    return ForcedMouseAndKeyboardSimplifiedInputDevice;
                case InputDetectionMode.GamePad:
                    if (activeSimplifiedInputDevice.deviceType == SimplifiedInputDeviceType.Gamepad)
                    {
                        return activeSimplifiedInputDevice;
                    }
                    else
                    {
                        return ForcedGamepadInputDevice;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        private set
        {
            activeSimplifiedInputDevice = value;
        }
    }
    public InputActionAsset InputActionAsset => inputActionAsset;

    protected override bool Init()
    {
        ///
        forceMouseAndKeyBoard = true;
        Debug.LogWarning("Forcing mouse and keyboard input");

        ///
        DetectInputDevice();

        ///
        return true;
    }

    public void OnDestroy()
    {
        InputSystem.onEvent -= InputSystem_onEvent;
        InputSystem.onDeviceChange -= InputSystem_onDeviceChange;
    }

    protected override void ExtendedAwake()
    {
        InputSystem.onEvent += InputSystem_onEvent;
        InputSystem.onDeviceChange += InputSystem_onDeviceChange;
    }

    private void InputSystem_onDeviceChange(InputDevice arg1, InputDeviceChange arg2)
    {
        var deviceType = SimplifiedDeviceRecognition.GetSimplifiedInputDevice(arg1).deviceType;

        ///
        if (deviceType == SimplifiedInputDeviceType.Gamepad
            && (arg2 == InputDeviceChange.Disabled
            || arg2 == InputDeviceChange.Disconnected
            || arg2 == InputDeviceChange.Removed))
        {
            OnAnyGamepadDisconnected?.Invoke(arg1);
        }
    }

    public void InvokeOnActiveInputDeviceChanged()
    {
        OnActiveInputDeviceChanged?.Invoke();
    }

    private void InputSystem_onEvent(UnityEngine.InputSystem.LowLevel.InputEventPtr arg1, InputDevice inputDevice)
    {
        ///
        if (activeInputDeviceId != inputDevice.deviceId)
        {
            ///
            var savedDeviceType = ActiveSimplifiedInputDevice.deviceType;

            ///
            ActiveSimplifiedInputDevice = SimplifiedDeviceRecognition.GetSimplifiedInputDevice(inputDevice);
            activeInputDeviceId = inputDevice.deviceId;

            ///
            OnActiveInputDeviceChanged?.Invoke();

            ///
            if (savedDeviceType != ActiveSimplifiedInputDevice.deviceType)
            {
                OnSimplifiedInputDeviceTypeChanged?.Invoke();
            }
        }

        ///
        if (GameSetting.ForceMouseAndKeyboard
            && activeSimplifiedInputDevice.deviceType == SimplifiedInputDeviceType.Gamepad)
        {
            InputSystem.ResetDevice(inputDevice, true);
            InputSystem.DisableDevice(inputDevice);
            InputSystem.RemoveDevice(inputDevice);

            ///
            return;
        }
    }

    private void DetectInputDevice()
    {
        ///
        int i = -1;
        int supportLevel = -1;

        ///
        foreach (var inputDevice in InputSystem.devices)
        {
            ///
            i++;

            ///
            var simplifiedDevice = SimplifiedDeviceRecognition.GetSimplifiedInputDevice(inputDevice);

            ///
            if (i == 0)
            {
                activeSimplifiedInputDevice = simplifiedDevice;
                supportLevel = simplifiedDevice.SupportLevel;
            }
            else
            {
                var newSupportLevel = simplifiedDevice.SupportLevel;
                if (newSupportLevel > supportLevel)
                {
                    activeSimplifiedInputDevice = simplifiedDevice;
                    supportLevel = newSupportLevel;
                }
            }

        }
    }
}

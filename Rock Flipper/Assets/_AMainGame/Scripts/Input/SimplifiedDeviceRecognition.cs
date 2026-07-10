using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// using UnityEngine.InputSystem.Switch;

public static class SimplifiedDeviceRecognition
{
    public static bool LogDetection = false;

    private const string MouseLayout = "Mouse";
    private const string KeyboardLayout = "Keyboard";
    private const string XboxLayout = "XInputController";
    private const string PsLayout = "DualShockGamepad";
    private const string SwitchLayout = "SwitchProControllerHID";

    public static SimplifiedInputDevice GetSimplifiedInputDevice(InputDevice inputDevice)
    {
        ///
        var deviceName = inputDevice.layout;

        ///
        SimplifiedInputDevice simplifiedInputDevice;

        ///
        if (InputSystem.IsFirstLayoutBasedOnSecond(deviceName, MouseLayout) || InputSystem.IsFirstLayoutBasedOnSecond(deviceName, KeyboardLayout))
        {
            simplifiedInputDevice = new SimplifiedInputDevice()
            {
                deviceType = SimplifiedInputDeviceType.MouseAndKeyboard,
                gamepadType = SimplifiedGamepadType.None,
                originalDeviceName = deviceName
            };
        }
        else
        {
            ///
            var deviceType = SimplifiedInputDeviceType.Gamepad;
            var gamepadType = SimplifiedGamepadType.Other;

            ///
            if (InputSystem.IsFirstLayoutBasedOnSecond(deviceName, XboxLayout))
            {
                gamepadType = SimplifiedGamepadType.Xbox;
            }
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceName, PsLayout))
            {
                gamepadType = SimplifiedGamepadType.PS;
            }
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceName, SwitchLayout))
            {
                gamepadType = SimplifiedGamepadType.Switch;
            }

            ///
            simplifiedInputDevice = new SimplifiedInputDevice()
            {
                deviceType = deviceType,
                gamepadType = gamepadType,
                originalDeviceName = deviceName
            };
        }

        ///
        if (LogDetection)
        {
            Debug.LogFormat("Input layout: {0} - {1} - {2}", deviceName, simplifiedInputDevice.deviceType, simplifiedInputDevice.gamepadType);
        }

        ///
        return simplifiedInputDevice;
    }
}

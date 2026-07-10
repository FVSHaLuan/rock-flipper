using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct SimplifiedInputDevice
{
    public SimplifiedInputDeviceType deviceType;
    public SimplifiedGamepadType gamepadType;

    public string originalDeviceName;

    public int SupportLevel
    {
        get
        {
            ///
            int level;

            ///
            if (Application.isConsolePlatform)
            {
                level = deviceType == SimplifiedInputDeviceType.Gamepad ? 200 : 100;
            }
            else            
            {
                level = deviceType == SimplifiedInputDeviceType.Gamepad ? 100 : 200;
            }            

            ///
            if (deviceType == SimplifiedInputDeviceType.Gamepad)
            {
                switch (gamepadType)
                {
                    case SimplifiedGamepadType.Xbox:
                        level += 30;
                        break;
                    case SimplifiedGamepadType.PS:
                        level += 20;
                        break;
                    case SimplifiedGamepadType.Switch:
                        level += 10;
                        break;
                    case SimplifiedGamepadType.Other:
                        break;
                    case SimplifiedGamepadType.None:
                        break;
                    default:
                        break;
                }
            }

            ///
            return level;
        }
    }
}

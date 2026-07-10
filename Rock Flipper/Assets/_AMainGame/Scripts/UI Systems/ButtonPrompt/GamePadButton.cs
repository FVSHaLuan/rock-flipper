using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI.ButtonPrompts
{
    public enum GamepadButton
    {
        NotSet = 0,
        // Triggers
        LeftTrigger = 1,
        RightTrigger = 2,
        // Shoulders
        LeftShoulder = 3,
        RightShoulder = 4,
        // Sticks
        LeftStick = 5,
        LeftStickPress = 6,
        RightStick = 7,
        RightStickPress = 8,
        // Arrow buttons
        Left = 9,
        Right = 10,
        Up = 11,
        Down = 12,
        // Direction buttons
        North = 13,
        East = 14,
        South = 15,
        West = 16,
        // Functionalities
        GameplayMenu = 17,
        SystemMenu = 18,
    }
}
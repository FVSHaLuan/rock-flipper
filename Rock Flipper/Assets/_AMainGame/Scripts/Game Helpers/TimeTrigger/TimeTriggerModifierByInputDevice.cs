using FH.Core.Gameplay.HelperComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeTrigger))]
public class TimeTriggerModifierByInputDevice : ExtendedMonoBehaviour
{
    [Header("Mouse and keyboard")]
    [SerializeField]
    private float mouseAndKeyboardTime;

    [Header("Game pad")]
    [SerializeField]
    private float gamepadTime;

    private TimeTrigger timeTrigger;

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        timeTrigger = GetComponent<TimeTrigger>();
        timeTrigger.OnBeforeCountingDown += TimeTrigger_OnBeforeCountingDown;
    }

    private void TimeTrigger_OnBeforeCountingDown()
    {
        ///
        var inputDeviceType = entry.inputManager.ActiveSimplifiedInputDevice.deviceType;

        ///
        switch (inputDeviceType)
        {
            case SimplifiedInputDeviceType.MouseAndKeyboard:
                timeTrigger.CountDownTime = mouseAndKeyboardTime;
                break;
            case SimplifiedInputDeviceType.Gamepad:
                timeTrigger.CountDownTime = gamepadTime;
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

#if UNITY_EDITOR
    private void Reset()
    {
        mouseAndKeyboardTime = gamepadTime = GetComponent<TimeTrigger>().CountDownTime;
    }
#endif
}

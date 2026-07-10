using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Card3DInspectionEffectWithGamepad : Card3DInspectionEffect
{
    [SerializeField]
    private bool isInspectingByMouse;
    [SerializeField]
    private bool isInspectingByGamepad;

    [Space]
    [SerializeField]
    private InputActionReference stickInputActionRef;

    public bool IsInspectingByGamePad
    {
        get => isInspectingByGamepad;
        set => isInspectingByGamepad = value;
    }

    public bool IsInspectingByMouse
    {
        get => isInspectingByMouse;
        set => isInspectingByMouse = value;
    }

    protected override Vector2 UpdateNormalizedInput()
    {
        ///
        if (!Entry.Instance.GameSetting.enabledCard3DInspectorEffect)
        {
            return StillInput;
        }

        ///
        if (Entry.Instance.inputManager.ActiveSimplifiedInputDevice.deviceType == SimplifiedInputDeviceType.MouseAndKeyboard)
        {
            if (IsInspectingByMouse)
            {
                return base.UpdateNormalizedInput();
            }
            else
            {
                return StillInput;
            }
        }
        else
        {
            if (IsInspectingByGamePad)
            {
                return GetNormalizedInputByGamePad();
            }
            else
            {
                return StillInput;
            }
        }
    }

    public override void Animate()
    {
        ///
        if (!Entry.Instance.GameSetting.enabledCard3DInspectorEffect)
        {
            return;
        }

        ///
        base.Animate();
    }

    private Vector2 GetNormalizedInputByGamePad()
    {
        if (stickInputActionRef == null)
        {
            return StillInput;
        }

        ///
        var v = stickInputActionRef.action.ReadValue<Vector2>();

        ///
        v.x = Mathf.InverseLerp(-1, 1, v.x);
        v.y = Mathf.InverseLerp(-1, 1, v.y);

        ///
        return v;
    }
}

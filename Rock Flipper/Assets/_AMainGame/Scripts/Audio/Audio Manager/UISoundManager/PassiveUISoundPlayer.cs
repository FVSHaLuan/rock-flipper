using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class PassiveUISoundPlayer : ExtendedMonoBehaviour
{
    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        entry.inputSystemUIInputModule.submit.action.performed += Action_performed;
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ///
        if (!ShouldPlaySoundForCurrentSelectable())
        {
            return;
        }

        ///
        entry.uiSoundManager.PlayPressSound();
    }

    private bool ShouldPlaySoundForCurrentSelectable()
    {
        ///
        var selectable = entry.uiSelectedEventManager.LastSelectable;

        ///
        if (selectable == null)
        {
            return false;
        }

        ///
        if (!selectable.isActiveAndEnabled)
        {
            return false;
        }

        ///
        if (selectable == null)
        {
            return false;
        }

        ///
        var selectableInfo = entry.uiSelectedEventManager.LastSelectableInfo;

        ///
        if (selectableInfo != null && selectableInfo.IgnorePassiveSoundPlayer)
        {
            return false;
        }

        ///
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using UnityEngine.InputSystem;

public class ButtonEnhancedScrollerMouse : ButtonScrollerMouse
{
    [Header("ButtonEnhancedScrollerMouse")]
    [SerializeField]
    private EnhancedScroller enhancedScroller;

    protected override void UiInputActionBase_OnActionPerformedIgnoredStartedTime(InputAction.CallbackContext context)
    {
        ///
        rectTransformToScroll = enhancedScroller.Container;

        ///
        base.UiInputActionBase_OnActionPerformedIgnoredStartedTime(context);
    }
}

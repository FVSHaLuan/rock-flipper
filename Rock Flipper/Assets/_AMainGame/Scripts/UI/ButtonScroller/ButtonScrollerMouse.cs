using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIInputActionBase))]
public class ButtonScrollerMouse : ExtendedMonoBehaviour
{
    [SerializeField]
    protected RectTransform rectTransformToScroll;
    [SerializeField]
    private RectTransform viewPort;
    [SerializeField]
    private float scrollDelta = 100;

    private UIInputActionBase uiInputActionBase;

    protected override void ExtendedAwake()
    {
        ///
        uiInputActionBase = GetComponent<UIInputActionBase>();

        ///       
        uiInputActionBase.OnActionPerformedIgnoredStartedTime += UiInputActionBase_OnActionPerformedIgnoredStartedTime;
    }

    protected virtual void UiInputActionBase_OnActionPerformedIgnoredStartedTime(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ///
        if (rectTransformToScroll == null)
        {
            return;
        }

        ///
        var value = context.ReadValue<Vector2>();

        ///
        float maxY = rectTransformToScroll.rect.height - viewPort.rect.height;
        if (maxY < 0)
        {
            maxY = 0;
        }

        ///
        var p = rectTransformToScroll.anchoredPosition;
        p.y += -value.y * scrollDelta;
        p.y = Mathf.Clamp(p.y, 0, maxY);
        rectTransformToScroll.anchoredPosition = p;
    }
}

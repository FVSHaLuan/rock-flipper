using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIInputActionBase))]
public class ButtonScrollerGamepad : ExtendedMonoBehaviour
{
    public const float MinInputVelocity = 0.1f;

    [SerializeField]
    protected RectTransform rectTransformToScroll;
    [SerializeField]
    private RectTransform viewPort;
    [SerializeField]
    private float maxScrollSpeed = 500;

    private UIInputActionBase uiInputActionBase;

    private float velocity = 0;

    protected override void ExtendedAwake()
    {
        ///
        uiInputActionBase = GetComponent<UIInputActionBase>();

        ///       
        uiInputActionBase.OnActionPerformedIgnoredStartedTime += UiInputActionBase_OnActionPerformedIgnoredStartedTime;
        uiInputActionBase.OnActionCanceled += UiInputActionBase_OnActionCanceled;
        uiInputActionBase.OnActionDisrupted += UiInputActionBase_OnActionDisrupted;
    }

    private void UiInputActionBase_OnActionDisrupted(bool performed, float durationFromStarted, float durationFromPerformed)
    {
        velocity = 0;
    }

    private void UiInputActionBase_OnActionCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        velocity = 0;
    }

    private void UiInputActionBase_OnActionPerformedIgnoredStartedTime(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ///
        var value = context.ReadValue<Vector2>();

        ///
        if (Mathf.Abs(value.y) <= MinInputVelocity)
        {
            velocity = 0;
            return;
        }

        ///
        velocity = -value.y * maxScrollSpeed;
    }

    public virtual void Update()
    {
        ///
        if (rectTransformToScroll == null)
        {
            return;
        }

        ///
        if (Mathf.Approximately(velocity, 0))
        {
            return;
        }

        ///
        float maxY = rectTransformToScroll.rect.height - viewPort.rect.height;
        if (maxY < 0)
        {
            maxY = 0;
        }

        ///
        var p = rectTransformToScroll.anchoredPosition;
        p.y += velocity * Time.unscaledDeltaTime;
        p.y = Mathf.Clamp(p.y, 0, maxY);

        ///
        rectTransformToScroll.anchoredPosition = p;
    }
}

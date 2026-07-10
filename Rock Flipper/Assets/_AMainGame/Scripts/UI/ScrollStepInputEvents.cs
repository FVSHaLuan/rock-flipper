using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScrollStepInputEvents : UIScreenChildInteraction
{
    [SerializeField]
    private float minGamePadInterval = 1 / 10.0f;
    [SerializeField]
    private float maxGamePadInterval = 1 / 6.0f;

    [Space]
    [SerializeField]
    private UnityEvent onStepUp;
    [SerializeField]
    private UnityEvent onStepDown;

    private float lastTimeGamePadEvent = -1;

    protected void OnEnable()
    {
        entry.inputManager.inputInfoScrollMouse.action.performed += OnMouse_performed;
        entry.inputManager.inputInfoScrollGamePad.action.performed += OnGamepad_performed;
    }

    protected void OnDisable()
    {
        entry.inputManager.inputInfoScrollMouse.action.performed -= OnMouse_performed;
        entry.inputManager.inputInfoScrollGamePad.action.performed -= OnGamepad_performed;
    }

    private void OnGamepad_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ///
        if (!Interactable)
        {
            return;
        }

        ///
        var value = context.ReadValue<Vector2>();

        ///
        if (Mathf.Abs(value.y) <= ButtonScrollerGamepad.MinInputVelocity)
        {
            return;
        }

        ///
        float effectiveInterval = Mathf.Lerp(minGamePadInterval, maxGamePadInterval, (Mathf.Abs(value.y) - ButtonScrollerGamepad.MinInputVelocity) / (1 - ButtonScrollerGamepad.MinInputVelocity));

        ///
        if ((Time.unscaledTime - lastTimeGamePadEvent) < effectiveInterval)
        {
            return;
        }

        ///
        if (value.y < 0)
        {
            ///
            lastTimeGamePadEvent = Time.unscaledTime;

            ///
            onStepDown?.Invoke();
        }
        else if (value.y > 0)
        {
            ///
            lastTimeGamePadEvent = Time.unscaledTime;

            ///
            onStepUp?.Invoke();
        }
        else
        {
            lastTimeGamePadEvent = -1;
        }
    }

    private void OnMouse_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ///
        if (!Interactable)
        {
            return;
        }

        ///
        var value = context.ReadValue<Vector2>();

        ///
        if (value.y < 0)
        {
            onStepDown?.Invoke();
        }
        else if (value.y > 0)
        {
            onStepUp?.Invoke();
        }
    }
}

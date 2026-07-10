using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequentialUIInputActions : ExtendedMonoBehaviour
{
    [SerializeField]
    private Transform actionRoot;

    [Space]
    [SerializeField]
    private UnityEvent onFinished;

    private List<UIInputActionBase> uIInputActions;

    private int currentIndex = -1;

    private bool rightKeyhit;
    private bool anyKeyHit;

    protected override bool Init()
    {
        ///
        GetUIInputActions();

        ///
        return base.Init();
    }

    protected void OnEnable()
    {
        ///
        StartOver();
    }

    protected void OnDisable()
    {
        ///
        SetCurrentIndex(-1);

        ///
        entry.anyKeyDetector.OnAnyKeyPressedThisFrame -= AnyKeyDetector_OnAnyKeyPressedThisFrame;
    }

    protected void LateUpdate()
    {
        ///
        if (uIInputActions.Count == 0)
        {
            return;
        }

        ///
        if (rightKeyhit)
        {
            ///
            var nextIndex = currentIndex + 1;

            ///
            if (nextIndex >= uIInputActions.Count)
            {
                ///
                SetCurrentIndex(-1);

                ///
                onFinished?.Invoke();
            }
            else
            {
                SetCurrentIndex(nextIndex);
            }
        }
        else if (anyKeyHit)
        {
            SetCurrentIndex(0);
        }

        ///
        rightKeyhit = false;
        anyKeyHit = false;
    }

    private void StartOver()
    {
        ///
        foreach (var item in uIInputActions)
        {
            item.gameObject.SetActive(false);
        }

        ///
        if (uIInputActions.Count > 0)
        {
            SetCurrentIndex(0);
        }

        ///
        anyKeyHit = false;
        rightKeyhit = false;

        ///
        entry.anyKeyDetector.OnAnyKeyPressedThisFrame += AnyKeyDetector_OnAnyKeyPressedThisFrame;
    }

    private void AnyKeyDetector_OnAnyKeyPressedThisFrame()
    {
        anyKeyHit = true;
    }

    private void SetCurrentIndex(int index)
    {
        ///
        if (currentIndex >= 0)
        {
            var uiInputAction = uIInputActions[currentIndex];
            uiInputAction.OnActionPerformed -= UiInputAction_OnActionPerformed;
            uiInputAction.gameObject.SetActive(false);
        }

        ///
        currentIndex = index;
        if (index >= 0)
        {
            var uiInputAction = uIInputActions[currentIndex];
            uiInputAction.OnActionPerformed += UiInputAction_OnActionPerformed;
            uiInputAction.gameObject.SetActive(true);
        }
    }

    private void UiInputAction_OnActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        rightKeyhit = true;
    }

    private void GetUIInputActions()
    {
        ///
        uIInputActions = new List<UIInputActionBase>();

        ///
        actionRoot.GetComponentsInChildren(true, uIInputActions);
    }

#if UNITY_EDITOR
    protected void Reset()
    {
        actionRoot = transform;
    }
#endif
}

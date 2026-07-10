using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorVisibilityManager : ExtendedMonoBehaviour
{
    private const bool AlwaysShowMouseCursor = true; // true when game does not support controller input

    private BalancerWithObjects hideMouseCursorBalancer = new BalancerWithObjects();

    private bool showEvenOnGamePad = false;

    public bool ShowEvenOnGamePad
    {
        get => showEvenOnGamePad;

        set
        {
            ///
            showEvenOnGamePad = value;

            ///
            UpdateVisibility();
        }
    }

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        UpdateVisibility();

        ///
        hideMouseCursorBalancer.OnBalanceChanged += HideMouseCursorBalancer_OnBalanceChanged;
        entry.inputManager.OnActiveInputDeviceChanged += InputManager_OnActiveInputDeviceChanged;

        ///
        if (!IsLoadingScreenNullOrFinished)
        {
            Agame.UI.LoadingScreen.LoadingScreenHandle.Instance.OnFinished += Instance_OnFinished;
        }
    }

    private void Instance_OnFinished()
    {
        UpdateVisibility();
    }

    private void InputManager_OnActiveInputDeviceChanged()
    {
        UpdateVisibility();
    }

    public void AddShowMouseCursorLock(object @object)
    {
        hideMouseCursorBalancer.AddObject(@object);
    }

    public void RemoveShowMouseCursorLock(object @object)
    {
        hideMouseCursorBalancer.RemoveObject(@object);
    }

    private void HideMouseCursorBalancer_OnBalanceChanged()
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (AlwaysShowMouseCursor)
        {
            ///
            Cursor.visible = true;

            ///
            return;
        }

        ///
        if (!IsLoadingScreenNullOrFinished)
        {
            ///
            Cursor.visible = false;

            ///
            return;
        }

        ///
        if (hideMouseCursorBalancer.IsBalanced)
        {
            ///
            Cursor.visible = false;

            ///
            return;
        }

        ///
        if (entry.inputManager.ActiveSimplifiedInputDevice.deviceType == SimplifiedInputDeviceType.MouseAndKeyboard || showEvenOnGamePad)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }
}

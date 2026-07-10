using System.Collections;
using System.Collections.Generic;
using Agame.FeatureBranching;
using UnityEngine;

/// <summary>
/// Obsolete, not worth solving!
/// </summary>
[System.Obsolete]
public class InputDeviceFlickeringResolver : ExtendedMonoBehaviour
{
    [SerializeField]
    private int changeCount = 5;
    [SerializeField]
    private float changeDuration = 2;

    private float timeAccount;

    protected void OnDestroy()
    {
        entry.inputManager.OnSimplifiedInputDeviceTypeChanged -= InputManager_OnSimplifiedInputDeviceTypeChanged;
    }

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        timeAccount = changeDuration;

        ///
        entry.inputManager.OnSimplifiedInputDeviceTypeChanged += InputManager_OnSimplifiedInputDeviceTypeChanged;
    }

    private void InputManager_OnSimplifiedInputDeviceTypeChanged()
    {
        timeAccount -= (changeDuration / changeCount);

        ///
        if (timeAccount <= 0)
        {
            HandleOverThreshold();
        }
    }

    private void HandleOverThreshold()
    {
        if (PlatformBranchInfo.Current == PlatformBranch.PC
                        || PlatformBranchInfo.Current == PlatformBranch.Mac
                        // || PlatformBranchInfo.Current == PlatformBranch.Linux   ****: DO NOT change on Linux because of Steam Deck
                        || PlatformBranchInfo.Current == PlatformBranch.Web)
        {
            SettleOn(InputDetectionMode.MouseAndKeyBoard);
        }
        else if (PlatformBranchInfo.Current == PlatformBranch.XBox)
        {
            SettleOn(InputDetectionMode.GamePad);
        }
    }

    protected void Update()
    {
        ///
        if (gameSetting.InputDetectionMode != InputDetectionMode.Auto)
        {
            return;
        }

        ///
        timeAccount += Time.unscaledDeltaTime;
        if (timeAccount > changeDuration)
        {
            timeAccount = changeDuration;
        }
    }

    private void SettleOn(InputDetectionMode inputDetectionMode)
    {
        gameSetting.InputDetectionMode = inputDetectionMode;
        timeAccount = changeDuration;
    }
}

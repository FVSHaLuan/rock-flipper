using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInputAction : UIInputActionBase
{
    [Header("UIInputAction")]
    [SerializeField]
    protected bool playSound = true;

    protected override void Awake()
    {
        ///
        base.Awake();

        ///
        StartWith(GetComponentInParent<UIScreen>());

        ///
        OnActionPerformed += UIInputAction_OnActionPerformed;
    }

    private void UIInputAction_OnActionPerformed(InputAction.CallbackContext obj)
    {
        if (playSound)
        {
            PlayPressSound();
        }
    }

    protected virtual void PlayPressSound()
    {
        Entry.Instance.uiSoundManager.PlayPressSound();
    }
}

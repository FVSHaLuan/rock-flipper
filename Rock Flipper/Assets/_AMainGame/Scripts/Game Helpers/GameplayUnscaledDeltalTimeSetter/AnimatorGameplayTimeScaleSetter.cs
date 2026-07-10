using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class AnimatorGameplayTimeScaleSetter : GameplayTimeScaleSetter
{
    private Animator animator;

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        animator = GetComponent<Animator>();
    }

    protected override void Set(bool useUnscaledTime)
    {
        animator.updateMode = useUnscaledTime ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
    }
}

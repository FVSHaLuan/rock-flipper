using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class AnimatorHelper : MonoBehaviour
{
    private Animator animator;

    private void TryGetAnimator()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void ResetAllTriggers()
    {
        ///
        TryGetAnimator();

        ///
        foreach (var item in animator.parameters)
        {
            if (item.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(item.name);
            }
        }
    }

    public void ResetAndSetTrigger(string trigger)
    {
        ///
        TryGetAnimator();

        ///
        ResetAllTriggers();

        ///
        animator.SetTrigger(trigger);
    }
}

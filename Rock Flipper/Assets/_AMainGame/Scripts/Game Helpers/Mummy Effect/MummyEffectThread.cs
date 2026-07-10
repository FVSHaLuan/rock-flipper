using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MummyEffectThread : GeneralPoolMemberSimplified
{
    [SerializeField]
    private UnityEvent onDisappearDelegate;

    [ContextMenu("Disappear")]
    public void Disappear()
    {
        onDisappearDelegate?.Invoke();
    }
}

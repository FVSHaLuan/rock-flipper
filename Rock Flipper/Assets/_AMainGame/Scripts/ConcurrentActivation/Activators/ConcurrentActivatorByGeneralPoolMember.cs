using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcurrentActivatorByGeneralPoolMember : ConcurrentActivator
{
    private GeneralPoolMemberSimplified generalPoolMemberSimplified;

    protected override bool Init()
    {
        ///
        generalPoolMemberSimplified = GetComponentInParent<GeneralPoolMemberSimplified>();

        ///
        if (generalPoolMemberSimplified == null)
        {
            Debug.LogError("GeneralPoolMemberSimplified not found", this);
        }

        ///
        return base.Init();
    }

    protected override ConcurrentActivationManager.IKeyState GetKeyState()
    {
        ///
        TryInit();

        ///
        return Entry.Instance.concurrentActivationManager.GetKeyStateByGeneralPoolPrototypeId(generalPoolMemberSimplified.PrototypeId);
    }

    protected override void UpdateWithActivation()
    {
        Entry.Instance.concurrentActivationManager.UpdateWithNewActivationByGeneralPoolPrototypeId(generalPoolMemberSimplified.PrototypeId);
    }

#if UNITY_EDITOR
    protected void Reset()
    {
        if (GetComponentInParent<GeneralPoolMemberSimplified>() == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("notice", "GeneralPoolMemberSimplifed not found on any of parents", "OK");
            DestroyImmediate(this);
        }
    }
#endif
}

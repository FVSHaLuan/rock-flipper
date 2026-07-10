using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT.Dev;

public class ExtendedMonoBehaviourWithUniqueId : ExtendedMonoBehaviour
{
    [SerializeField, ReadOnly]
    private int uniqueId = UniqueIntManager.InvalidId;

    public int UniqueId => uniqueId;

    protected virtual void Reset()
    {
        ///
#if UNITY_EDITOR
        if (uniqueId == UniqueIntManager.InvalidId)
        {
            uniqueId = DevEntry.Instance.uniqueIntManager.GetNextId();
        }
#endif
    }

    public void Editor_AssignNewUniqueId()
    {
#if UNITY_EDITOR
        ///
        UnityEditor.Undo.RecordObject(this, "Editor_AssignNewUniqueId");

        ///
        uniqueId = DevEntry.Instance.uniqueIntManager.GetNextId();

        ///
        Debug.Log("Set new unique Id");
#endif
    }

    protected virtual void OnValidate()
    {
        Editor_DetectDuplicatedId();
    }

    private void Editor_DetectDuplicatedId()
    {
#if UNITY_EDITOR
        ///
        var listInScene = FindObjectsByType<ExtendedMonoBehaviourWithUniqueId>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var listFromRoot = transform.root.GetComponentsInChildren<ExtendedMonoBehaviourWithUniqueId>(true);

        ///
        bool isDuplicated = false;

        ///
        foreach (var item in listInScene)
        {
            if (item != this && item.uniqueId == this.uniqueId)
            {
                isDuplicated = true;
                break;
            }
        }

        ///
        if (!isDuplicated)
        {
            foreach (var item in listFromRoot)
            {
                if (item != this && item.uniqueId == this.uniqueId)
                {
                    isDuplicated = true;
                    break;
                }
            }
        }

        ///
        if (isDuplicated)
        {
            ///
            uniqueId = DevEntry.Instance.uniqueIntManager.GetNextId();

            ///
            Debug.LogFormat("Found duplicated id. Find object: {0}. Changed the Id.", name);
        }
#endif
    }
}

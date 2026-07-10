using Agame.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectWithUniqueId : ScriptableObjectWithInit, GD.IUnique<string>
{
    [SerializeField, ReadOnly]
    private string uniqueId;

    public virtual string UniqueId => uniqueId;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (this.HasAssetPath())
        {
            if (!string.IsNullOrWhiteSpace(uniqueId))
            {
                UniqueIdValidator.Validate(uniqueId, this);
            }
            else
            {
                Editor_ForceAssignId();
            }
        }
    }

    protected virtual void Reset()
    {
        Editor_AssignUniqueId();
    }

    [ContextMenu("Editor_AssignId")]
    public void Editor_AssignUniqueId()
    {
        ///
        if (!string.IsNullOrWhiteSpace(uniqueId))
        {
            if (!UnityEditor.EditorUtility.DisplayDialog("And Id is assigned already.", "Are you sure you want a new Id?", "OK", "Cancel"))
            {
                return;
            }
        }

        ///
        Editor_ForceAssignId();
    }

    private void Editor_ForceAssignId()
    {
        ///
        UnityEditor.Undo.RecordObject(this, "AssignId");

        ///
        if (uniqueId != null)
        {
            UniqueIdValidator.RemoveEntry(uniqueId, this);
        }

        ///
        uniqueId = DevEntry.Instance.uniqueIntManager.GetNextId().ToString();

        ///
        UnityEditor.EditorUtility.SetDirty(this);
    }

    [ContextMenu("Editor_Duplicate")]
    public void Editor_Duplicate()
    {
        ///
        if (!EditorHelper.HasAssetPath(this))
        {
            ///
            Debug.LogError("Asset not has path yet");

            ///
            return;
        }

        ///
        var assetPath = AssetDatabase.GetAssetPath(this);

        ///
        var newAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

        ///
        var newAssetInstance = Object.Instantiate(this);

        ///
        newAssetInstance.uniqueId = DevEntry.Instance.uniqueIntManager.GetNextId().ToString();

        ///
        AssetDatabase.CreateAsset(newAssetInstance, newAssetPath);

        ///
        AssetDatabase.LoadAssetAtPath<ScriptableObjectWithUniqueId>(newAssetPath).OnValidate();
    }
#endif
}

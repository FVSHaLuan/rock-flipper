using FH.Core.Architecture.WritableData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Dev
{
    public abstract class PlayerDataSnapShotBase : ScriptableObject
    {
        [SerializeField, TextArea]
        private string note;

        [Space]
        [SerializeField, ReadOnly]
        private PlayerData savedPlayerData;

        [Space]
        [SerializeField, ReadOnly]
        private bool savedOnce = false;

        protected abstract PlayerDataObject PlayerDataObject { get; }

#if UNITY_EDITOR
        [ContextMenu("SaveCurrentState")]
        protected void SaveCurrentState()
        {
            ///
            if (savedOnce)
            {
                if (!UnityEditor.EditorUtility.DisplayDialog("Re-save", "Are you sure?", "OK", "Cancel"))
                {
                    return;
                }
            }

            ///
            UnityEditor.Undo.RecordObject(this, "SaveCurrentState");

            ///
            savedOnce = true;
            var playerData = PlayerDataObject.Data;
            savedPlayerData = BinarySerializationHelper.Clone(playerData);

            ///
            UnityEditor.EditorUtility.SetDirty(this);

            ///
            Debug.Log("Saved");
        }

        [ContextMenu("Restore")]
        protected void Restore()
        {
            ///
            PlayerDataObject.Data = BinarySerializationHelper.Clone(savedPlayerData);

            ///
            PlayerDataObject.SaveData();

            ///
            Debug.Log("Restored");
        }
#endif
    }

}
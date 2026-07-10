using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Dev
{
    [CreateAssetMenu(fileName = "UniqueIntManager", menuName = "BSB/Dev/UniqueIntManager")]
    public class UniqueIntManager : ScriptableObject
    {
        public const int InvalidId = int.MinValue;

        [ReadOnly]
        [SerializeField]
        private int nextId = int.MinValue + 1;

#if UNITY_EDITOR
        public int GetNextId()
        {
            ///
            var id = nextId;
            nextId++;

            UnityEditor.EditorUtility.SetDirty(this);

            ///
            return id;
        }
#endif
    }
}
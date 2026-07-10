using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Dev
{
    public class GameObjectStateConfiguration : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainGameObject;
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("sameObjectStateInfos")]
        private List<GameObjectStateInfo> gameObjectStateInfos = new List<GameObjectStateInfo>();

        [System.Serializable]
        public struct GameObjectStateInfo
        {
            public string name;
            public GameObjectBuildState gameObjectBuildState;
            public bool active;

            public GameObjectStateInfo UpdateName()
            {
                var rs = this;

                ///
                rs.name = string.Format("[{0}] {1}", active != gameObjectBuildState.DefaultActiveState ? active : "---", gameObjectBuildState.gameObject.name);

                ///
                return rs;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("SetState")]
        public void SetState()
        {
            ///
            foreach (var item in gameObjectStateInfos)
            {
                if (item.gameObjectBuildState != null)
                {
                    item.gameObjectBuildState.gameObject.SetActive(item.active);
                }
            }

            ///
            if (mainGameObject != null)
            {
                UnityEditor.Selection.activeObject = mainGameObject;
                UnityEditor.EditorGUIUtility.PingObject(mainGameObject);
            }
        }

        [ContextMenu("SetStateAddictive")]
        public void SetStateAddictive()
        {
            ///
            foreach (var item in gameObjectStateInfos)
            {
                if (item.active)
                {
                    item.gameObjectBuildState.gameObject.SetActive(true);
                }
            }
        }

        [ContextMenu("SaveState")]
        public void SaveState()
        {
            ///
            if (!UnityEditor.EditorUtility.DisplayDialog("SAVE STATE", "Are you sure you want to save current state?", "OK", "CANCEL"))
            {
                return;
            }

            ///
            UnityEditor.Undo.RecordObject(this, "SaveState");

            ///
            gameObjectStateInfos.Clear();

            ///
            var scene = gameObject.scene;

            ///
            var rootGameObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootGameObjects.Length; i++)
            {
                GetBuildStateForGameObjectsForTransform(rootGameObjects[i].transform);
            }

            ///
            UpdateNames();
        }

        private void GetBuildStateForGameObjectsForTransform(Transform transform)
        {
            ///
            var gameObjectBuildState = transform.GetComponent<GameObjectBuildState>();

            ///
            if (gameObjectBuildState != null && gameObjectBuildState.IncludedInConfiguration)
            {
                if (gameObjectBuildState.HasCorrectGameObjectName)
                {
                    ///
                    GameObjectStateInfo gameObjectStateInfo = new GameObjectStateInfo()
                    {
                        active = gameObjectBuildState.gameObject.activeSelf,
                        gameObjectBuildState = gameObjectBuildState
                    };

                    ///
                    gameObjectStateInfos.Add(gameObjectStateInfo);
                }
            }

            ///
            foreach (Transform item in transform)
            {
                if (item != transform)
                {
                    GetBuildStateForGameObjectsForTransform(item);
                }
            }
        }

        private void UpdateNames()
        {
            for (int i = 0; i < gameObjectStateInfos.Count; i++)
            {
                gameObjectStateInfos[i] = gameObjectStateInfos[i].UpdateName();
            }
        }

        [ContextMenu("UpdateNames")]
        private void UpdateNamesWithUndoRegister()
        {
            ///
            UnityEditor.Undo.RecordObject(this, "UpdateNames");

            ///
            UpdateNames();
        }
#endif
    }

}
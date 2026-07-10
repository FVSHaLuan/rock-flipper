using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;

namespace BT.Dev
{
    public static class GameObjectBuildStateSetter
    {
        static private List<GameObject> incorrectObjects = new List<GameObject>();

        [MenuItem("FH/Set BuildState for GameObjects")]
        public static void SetBuildStateForGameObjects()
        {
            ///
            bool isOK = EditorUtility.DisplayDialog("Set BuildState for GameObjects", "Set BuildState for GameObjects for all opening scenes?", "OK", "Cancel");

            ///
            incorrectObjects.Clear();

            ///
            if (!isOK)
            {
                return;
            }

            ///
            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);
                SetBuildStateForGameObjectsForScene(scene);
            }

            ///
            if (incorrectObjects.Count > 0)
            {
                ///
                Selection.objects = incorrectObjects.ToArray();

                ///
                Debug.LogErrorFormat("{0} incorrect objects", incorrectObjects.Count);
            }
        }

        private static void SetBuildStateForGameObjectsForScene(Scene scene)
        {
            ///
            EditorSceneManager.MarkSceneDirty(scene);

            ///
            var rootGameObjects = scene.GetRootGameObjects();
            for (int i = 0; i < rootGameObjects.Count(); i++)
            {
                SetBuildStateForGameObjectsForTransform(rootGameObjects[i].transform);
            }
        }

        private static void SetBuildStateForGameObjectsForTransform(Transform transform)
        {
            ///
            var gameObjectBuildStateSetter = transform.GetComponent<IGameObjectBuildStateSetter>();

            ///
            if (gameObjectBuildStateSetter != null)
            {
                if (!gameObjectBuildStateSetter.SetBuildState())
                {
                    incorrectObjects.Add(transform.gameObject);
                }
            }

            ///
            foreach (Transform item in transform)
            {
                if (item != transform)
                {
                    SetBuildStateForGameObjectsForTransform(item);
                }
            }
        }
    }

}
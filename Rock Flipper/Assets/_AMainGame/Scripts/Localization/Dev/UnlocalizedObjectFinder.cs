using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneLine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BT.Localization.Dev
{
    public class UnlocalizedObjectFinder : ScriptableObject
    {
        [SerializeField, OneLineWithHeader]
        private List<Directory> directories = new List<Directory>();

        [Space]
        [SerializeField]
        private bool findInScene = true;

        [System.Serializable]
        private struct Directory
        {
            public bool useThis;
            public Object directory;
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_Find")]
        private void Editor_Find()
        {
            ///
            HashSet<ILocalizable> localizableSet = new HashSet<ILocalizable>();
            List<ILocalizable> localizables = new List<ILocalizable>();

            // Find in all directories
            foreach (var item in directories)
            {
                ///
                if (!item.useThis)
                {
                    Debug.LogWarning("Directory not used: " + item.directory.name);
                    continue;
                }

                ///
                var path = AssetDatabase.GetAssetPath(item.directory);

                ///
                EditorHelper.GetAllObjetsFromPath<ILocalizable>(path, localizables);

                ///
                foreach (var l in localizables)
                {
                    if (!l.Editor_IsLocalized)
                    {
                        localizableSet.Add(l);
                    }
                }
            }

            // Find in scene
            if (findInScene)
            {
                ///
                var sceneLocalizables = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include);

                ///
                foreach (var l in sceneLocalizables)
                {
                    if (l is ILocalizable localizable && !localizable.Editor_IsLocalized)
                    {
                        localizableSet.Add(localizable);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Not searching in scene!");
            }

            ///
            if (localizableSet.Count == 0)
            {
                ///
                Debug.Log("Not found!");

                ///
                return;
            }

            ///
            Debug.LogFormat("Found {0} unlocalized objects:", localizableSet.Count);

            ///
            foreach (var item in localizableSet)
            {
                Debug.Log($"{(item as Object).name} ({item.GetType().Name})", item as Object);
            }
        }
#endif
    }

}
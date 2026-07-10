using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneLine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Agame.Localization.Dev
{
    public class UnlocalizedObjectFinder : ScriptableObject
    {
        [SerializeField, OneLineWithHeader]
        private List<Directory> directories = new List<Directory>();

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

            ///
            foreach (var item in directories)
            {
                ///
                if (!item.useThis)
                {
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
                Debug.LogFormat(item as Object, "{0}", (item as Object).name);
            }
        }
#endif
    }

}
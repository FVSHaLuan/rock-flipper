#if UNITY_EDITOR
using FH;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

class FindReferencesUtility
{
    [MenuItem("CONTEXT/Component/[From Root Only] Find references to this")]
    private static void FindReferencesFromRootOnly(MenuCommand data)
    {
        Object context = data.context;
        if (context)
        {
            var comp = context as Component;
            if (comp)
                FindReferencesTo(comp, true);
        }
    }

    [MenuItem("CONTEXT/Component/[From Root Only] Find references to GameObject")]
    private static void FindReferencesToGameObjectFromRootOnly(MenuCommand data)
    {
        Object context = data.context;
        if (context)
        {
            var comp = context as Component;
            if (comp)
            {
                FindReferencesTo(comp.gameObject, true);
            }
        }
    }

    [MenuItem("CONTEXT/Component/Find references to this")]
    private static void FindReferences(MenuCommand data)
    {
        Object context = data.context;
        if (context)
        {
            var comp = context as Component;
            if (comp)
                FindReferencesTo(comp, false);
        }
    }

    [MenuItem("CONTEXT/Component/Find references to GameObject")]
    private static void FindReferencesToGameObject(MenuCommand data)
    {
        Object context = data.context;
        if (context)
        {
            var comp = context as Component;
            if (comp)
            {
                FindReferencesTo(comp.gameObject, false);
            }
        }
    }

    [MenuItem("Assets/Find references to this")]
    private static void FindReferencesToAsset(MenuCommand data)
    {
        var selected = Selection.activeObject;
        if (selected)
            FindReferencesTo(selected, false);
    }

    private static void FindReferencesTo(Object to, bool rootOnly)
    {
        var referencedBy = new List<Object>();
        var allObjects = GetAllGameObjects(to, rootOnly);
        for (int j = 0; j < allObjects.Length; j++)
        {
            var go = allObjects[j];

            if (PrefabUtility.GetPrefabInstanceStatus(go) == PrefabInstanceStatus.Connected)
            {
                if (PrefabUtility.GetCorrespondingObjectFromSource(go) == to)
                {
                    FHLog.Log(string.Format("referenced by {0}, {1}", go.name, go.GetType()), go);
                    referencedBy.Add(go);
                }
            }

            var components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                var c = components[i];
                if (!c) continue;

                var so = new SerializedObject(c);
                var sp = so.GetIterator();

                while (sp.NextVisible(true))
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == to)
                        {
                            FHLog.Log(string.Format("referenced by {0}, {1}", c.name, c.GetType()), c);
                            referencedBy.Add(c.gameObject);
                        }
                    }
            }
        }

        if (referencedBy.Any())
            Selection.objects = referencedBy.ToArray();
        else FHLog.Log(string.Format("no references in scene. From Root Only = {0}", rootOnly));
    }

    private static GameObject[] GetAllGameObjects(Object to, bool rootOnly)
    {
        ///
        var go = to as GameObject;
        if (go == null)
        {
            go = (to as Component)?.gameObject;
        }
        if (rootOnly && go != null)
        {
            return GetAllGameObjectsFromRootOnly(go);
        }

        ///
        List<GameObject> allGameObjects = new List<GameObject>();

        ///
        GameObject[] roots;

        ///
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            ///
            roots = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects();

            ///
            foreach (var rootObject in roots)
            {
                var allTransforms = rootObject.GetComponentsInChildren<Transform>(true);
                foreach (var item in allTransforms)
                {
                    allGameObjects.Add(item.gameObject);
                }
            }
        }

        ///
        return allGameObjects.ToArray();
    }

    private static GameObject[] GetAllGameObjectsFromRootOnly(GameObject go)
    {
        ///
        List<GameObject> allGameObjects = new List<GameObject>();

        ///
        var root = go.transform.root;
        if (root == null)
        {
            root = go.transform;
        }

        ///
        var transforms = root.GetComponentsInChildren<Transform>(true);

        ///
        foreach (var item in transforms)
        {
            allGameObjects.Add(item.gameObject);
        }

        ///
        return allGameObjects.ToArray();
    }
}
#endif
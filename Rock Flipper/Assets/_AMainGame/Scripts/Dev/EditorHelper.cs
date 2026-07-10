using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorHelper
{
#if UNITY_EDITOR
    private static List<Action> executeInNextEditorUpdateActions = new List<Action>();
    private static List<Action> executeInNextEditorUpdateActionsTmp = new List<Action>();

    private static bool isExecuting = false;

    static EditorHelper()
    {
        UnityEditor.EditorApplication.update += EditorUpdate;
    }

    private static void EditorUpdate()
    {
        ///
        isExecuting = true;

        ///
        foreach (var item in executeInNextEditorUpdateActions)
        {
            try
            {
                item?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        ///
        executeInNextEditorUpdateActions.Clear();

        ///
        isExecuting = false;

        ///
        executeInNextEditorUpdateActions.AddRange(executeInNextEditorUpdateActionsTmp);
        executeInNextEditorUpdateActionsTmp.Clear();
    }

    public static void ExecuteInNextEditorUpdate(Action action)
    {
        if (!isExecuting)
        {
            executeInNextEditorUpdateActions.Add(action);
        }
        else
        {
            executeInNextEditorUpdateActionsTmp.Add(action);
        }
    }

    public static void GetAllObjetsFromPath<T>(string path, List<T> objects) where T : class
    {
        ///
        objects.Clear();

        ///
        GetAllScriptableObjetsFromPath(path, objects, false);
        GetAllComponentObjetsFromPath(path, objects, false);
    }

    private static void GetAllScriptableObjetsFromPath<T>(string path, List<T> objects, bool clearList) where T : class
    {
        ///
        if (clearList)
        {
            objects.Clear();
        }

        ///
        var searchString = "t: " + typeof(ScriptableObject).Name;
        var GUIDs = UnityEditor.AssetDatabase.FindAssets(searchString, new string[] { path });

        ///
        foreach (var guid in GUIDs)
        {
            ///
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

            ///
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath) as T;

            ///
            if (asset != null)
            {
                objects.Add(asset);
            }
        }
    }

    private static void GetAllComponentObjetsFromPath<T>(string path, List<T> objects, bool clearList)
    {
        ///
        if (clearList)
        {
            objects.Clear();
        }

        ///
        var searchString = "t: " + typeof(GameObject).Name;
        var GUIDs = UnityEditor.AssetDatabase.FindAssets(searchString, new string[] { path });

        ///
        foreach (var guid in GUIDs)
        {
            ///
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

            ///
            var gameObject = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            ///
            if (gameObject == null)
            {
                continue;
            }

            ///
            var comnponent = gameObject.GetComponent<T>();

            ///
            if (comnponent != null)
            {
                objects.Add(comnponent);
            }
        }

    }

    public static T GetFirstObjetFromPath<T>(string path) where T : UnityEngine.Object
    {
        ///
        var searchString = "t: " + typeof(T).Name;
        var GUIDs = UnityEditor.AssetDatabase.FindAssets(searchString, new string[] { path });

        ///
        foreach (var guid in GUIDs)
        {
            ///
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

            ///
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);

            ///
            if (asset != null)
            {
                return asset;
            }
        }

        ///
        return null;
    }

    public static T GetFirstObjetFromPathNonRecursive<T>(string path) where T : UnityEngine.Object
    {
        ///
        var files = System.IO.Directory.GetFiles(path, "*.asset");

        ///
        foreach (var filePath in files)
        {
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(filePath);
            if (asset == null)
            {
                continue;
            }

            ///
            return asset;
        }

        ///
        return null;
    }

#if UNITY_EDITOR
    public static bool HasAssetPath(this ScriptableObject scriptableObject)
    {
        var assetPath = UnityEditor.AssetDatabase.GetAssetPath(scriptableObject);
        if (string.IsNullOrWhiteSpace(assetPath))
        {
            return false;
        }

        ///
        return true;
    }
#endif
#endif
}

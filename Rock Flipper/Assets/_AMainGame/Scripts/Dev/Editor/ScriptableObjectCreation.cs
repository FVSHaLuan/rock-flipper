using BT.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BT.Dev
{
    public static class ScriptableObjectCreation
    {
        private delegate void CreationFunction(MonoScript monoScript);

        private static Dictionary<Type, CreationFunction> creationFunctionDictionary = new Dictionary<Type, CreationFunction>()
        {

        };

        [MenuItem("Assets/Create ScriptableObject Asset")]
        private static void CreateAsset(MenuCommand menuCommand)
        {
            ///
            var monoScript = Selection.objects[0] as MonoScript;

            ///
            var assetFilePath = GetAssetFilePath(monoScript, null, null);

            ///
            CreateAsset(monoScript, assetFilePath);
        }

        [MenuItem("Assets/Create ScriptableObject Asset", validate = true)]
        private static bool CreateAssetValidator()
        {
            ///
            if (Selection.count != 1)
            {
                return false;
            }

            ///
            var selectedObject = Selection.objects[0];

            ///
            var monoScript = selectedObject as MonoScript;

            ///
            if (monoScript == null)
            {
                return false;
            }

            ///
            var t = monoScript.GetClass();

            ///
            if (t.IsAbstract || !t.IsSubclassOf(typeof(ScriptableObject)))
            {
                return false;
            }

            ///
            return !IsOVCreatable(monoScript.GetClass());
        }

        [MenuItem("Assets/Create OV's Asset")]
        private static void CreateOVAsset(MenuCommand menuCommand)
        {
            ///
            var monoScript = Selection.objects[0] as MonoScript;

            ///
            var creationFunction = GetCreationFunction(monoScript.GetClass());

            ///
            creationFunction?.Invoke(monoScript);
        }

        [MenuItem("Assets/Create OV's Asset", validate = true)]
        public static bool CreateOVAssetValidator()
        {
            ///
            if (Selection.count != 1)
            {
                return false;
            }

            ///
            var selectedObject = Selection.objects[0];

            ///
            var monoScript = selectedObject as MonoScript;

            ///
            if (monoScript == null)
            {
                return false;
            }

            ///
            var t = monoScript.GetClass();

            ///
            if (t.IsAbstract || !t.IsSubclassOf(typeof(ScriptableObject)))
            {
                return false;
            }

            ///
            return IsOVCreatable(monoScript.GetClass());
        }

        private static bool IsOVCreatable(Type type)
        {
            ///
            foreach (var item in creationFunctionDictionary)
            {
                ///
                if (type.IsSubclassOf(item.Key))
                {
                    return true;
                }
            }

            ///
            return false;
        }

        private static CreationFunction GetCreationFunction(Type type)
        {
            ///
            foreach (var item in creationFunctionDictionary)
            {
                ///
                if (type.IsSubclassOf(item.Key))
                {
                    return item.Value;
                }
            }

            ///
            return null;
        }

        private static ScriptableObject CreateAsset(MonoScript monoScript, string assetFilePath)
        {
            var asset = CreateObject(monoScript);

            ///
            AssetDatabase.CreateAsset(asset, assetFilePath);
            AssetDatabase.Refresh();

            ///
            var savedObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetFilePath);

            ///
            Debug.LogFormat(savedObject, "Created asset at path: {0}", assetFilePath);

            ///
            return savedObject;
        }

        private static ScriptableObject CreateObject(MonoScript monoScript)
        {
            return ScriptableObject.CreateInstance(monoScript.GetClass());
        }

        /// <summary>
        /// Also create the path if needed
        /// </summary>
        /// <param name="monoScript"></param>
        /// <param name="scriptPath"></param>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        private static string GetAssetFilePath(MonoScript monoScript, string scriptPathParent, string assetPathParent)
        {
            ///
#if UNITY_EDITOR_OSX
            if (!string.IsNullOrEmpty(scriptPathParent))
            {
                scriptPathParent = scriptPathParent.Replace('\\', '/');
                assetPathParent = assetPathParent.Replace('\\', '/');
            }
#endif

            ///
            var scriptPath = AssetDatabase.GetAssetPath(monoScript);
            var scriptFolder = Path.GetDirectoryName(scriptPath);
            var assetFolder = string.IsNullOrEmpty(scriptPathParent) ? scriptFolder : scriptFolder.Replace(scriptPathParent, assetPathParent);

            ///
            Directory.CreateDirectory(assetFolder);

            ///
            var assetname = monoScript.name.Replace("Asset", string.Empty) + ".asset";
            var assetPath = Path.Combine(assetFolder, assetname);

            ///
            return assetPath;
        }
    }

}
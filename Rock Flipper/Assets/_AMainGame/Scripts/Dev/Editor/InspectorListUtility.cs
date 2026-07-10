/// <summary>
/// made by FVS, for Beat Stickman: Beyond project
/// Visit the game at: https://store.steampowered.com/app/1494280/Beat_Stickman_Beyond
/// ________________
/// ***Prerequisite:
/// - Place this file under an Editor folder in your Unity project
/// ________________
/// ***Copy a list from Unity to a text editor:
/// 1. Copy the list in the inspector
/// 2. Go to menu: FH/Inspector List/Convert to text list
/// 3. Now the list is in the clipboard, ready for you to paste in any text editor
/// ________________
/// ***Copy a list from a text editor to Unity:
/// 1. Copy the list in any text editor
/// 2. Go to menu: FH/Inspector List/Convert to Unity list: [ITEM TYPE]
/// 3. Now the list is in the clipboard, ready for you to paste to Unity's inspector
/// ________________
/// - Tested on Unity 2021.1.22f1
/// - Currently supports list of int, float, double and string
/// </summary>  

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Globalization;

public static class InspectorListUtility
{
    private const string UnityListPrefix = "GenericPropertyJSON:";
    private static char[] Separators = new char[] { '\n', '\r' };

    [System.Serializable]
    public class SerializedListNode<T> : IUnityListNode
    {
        public string name;
        public int type;
        public int arraySize;
        public string arrayType;
        public T val;
        public List<SerializedListNode<T>> children;

        public string Name => name;
        public int Type => type;
        public int ArraySize => arraySize;
        public string ArrayType => arrayType;
        public object Val => val;
        public IUnityListNode[] Children => children.ToArray();

        public string GetTextList()
        {
            ///
            if (children == null)
            {
                return null;
            }

            ///
            System.Text.StringBuilder strBd = new System.Text.StringBuilder();

            ///
            var realList = children[0].children;

            ///
            for (int i = 1; i < realList.Count; i++)
            {
                strBd.AppendLine(realList[i].val.ToString());
            }

            ///
            return strBd.ToString();
        }
    }

    public interface IUnityListNode
    {
        public string Name { get; }
        public int Type { get; }
        public int ArraySize { get; }
        public string ArrayType { get; }
        public object Val { get; }
        public IUnityListNode[] Children { get; }

        public string GetTextList();
    }

    [MenuItem("FH/Inspector List/Convert to text list")]
    public static void ConvertToTextList()
    {
        ///
        string json;

        ///
        if (!GetJson(GUIUtility.systemCopyBuffer, out json))
        {
            ///
            Debug.LogWarning("not found valid Unity list in clipboard");

            ///
            return;
        }

        ///
        var tmpList = JsonUtility.FromJson<SerializedListNode<int>>(json);

        ///
        IUnityListNode unityListNode = null;

        ///
        switch (tmpList.arrayType.ToUpper())
        {
            case "INT":
                unityListNode = JsonUtility.FromJson<SerializedListNode<int>>(json);
                break;
            case "STRING":
                unityListNode = JsonUtility.FromJson<SerializedListNode<string>>(json);
                break;
            case "FLOAT":
                unityListNode = JsonUtility.FromJson<SerializedListNode<float>>(json);
                break;
            case "DOUBLE":
                unityListNode = JsonUtility.FromJson<SerializedListNode<double>>(json);
                break;
            default:
                break;
        }

        ///
        var textList = unityListNode.GetTextList();

        ///
        Debug.LogFormat("Converted list of type {0}:\n{1}", unityListNode.ArrayType, textList);
        GUIUtility.systemCopyBuffer = textList;
    }

    [MenuItem("FH/Inspector List/Convert to Unity list: int")]
    public static void ConvertToUnityListInt()
    {
        ///
        var list = GetIntListFromClipboard();
        ConvertToUnityList(list, "int", 0, list.Count);
    }

    [MenuItem("FH/Inspector List/Convert to Unity list: float")]
    public static void ConvertToUnityListFloat()
    {
        ///
        var list = GetFloatListFromClipboard();
        ConvertToUnityList(list, "float", 2, list.Count);
    }

    [MenuItem("FH/Inspector List/Convert to Unity list: double")]
    public static void ConvertToUnityListDouble()
    {
        ///
        var list = GetDoubleListFromClipboard();
        ConvertToUnityList(list, "double", 2, list.Count);
    }

    [MenuItem("FH/Inspector List/Convert to Unity list: string")]
    public static void ConvertToUnityListString()
    {
        ///
        var list = GetStringListFromClipboard();
        ConvertToUnityList(list, "string", 3, list.Count.ToString());
    }

    private static List<int> GetIntListFromClipboard()
    {
        ///
        var list = new List<int>();

        ///
        var strList = GUIUtility.systemCopyBuffer.Split(Separators);

        ///
        for (int i = 0; i < strList.Length; i++)
        {
            ///
            var str = strList[i];

            ///
            if (int.TryParse(str, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int value))
            {
                list.Add(value);
            }
        }

        ///
        return list;
    }

    private static List<float> GetFloatListFromClipboard()
    {
        ///
        var list = new List<float>();

        ///
        var strList = GUIUtility.systemCopyBuffer.Split(Separators);

        ///
        for (int i = 0; i < strList.Length; i++)
        {
            ///
            var str = strList[i];

            ///
            if (float.TryParse(str, out float value))
            {
                list.Add(value);
            }
        }

        ///
        return list;
    }

    private static List<double> GetDoubleListFromClipboard()
    {
        ///
        var list = new List<double>();

        ///
        var strList = GUIUtility.systemCopyBuffer.Split(Separators);

        ///
        for (int i = 0; i < strList.Length; i++)
        {
            ///
            var str = strList[i];

            ///
            if (double.TryParse(str, out double value))
            {
                list.Add(value);
            }
        }

        ///
        return list;
    }

    private static List<string> GetStringListFromClipboard()
    {
        ///
        var strList = GUIUtility.systemCopyBuffer.Split(Separators, System.StringSplitOptions.RemoveEmptyEntries);

        ///
        return new List<string>(strList);
    }

    private static void ConvertToUnityList<T>(List<T> values, string typeName, int typeId, T valueCount)
    {
        ///
        SerializedListNode<T> serializedListNode = new SerializedListNode<T>()
        {
            arrayType = typeName,
            arraySize = values.Count,
            type = -1,
            name = "whatever",
            children = new List<SerializedListNode<T>>()
            {
                new SerializedListNode<T>()
                {
                     name="Array",
                     type=-1,
                     arraySize=values.Count,
                     arrayType=typeName,
                     children=new List<SerializedListNode<T>>()
                     {
                         new SerializedListNode<T>()
                         {
                             name="size",
                             type=12,
                             val=valueCount
                         }
                     }
                }
            },
        };

        ///
        var targetValueList = serializedListNode.children[0].children;

        ///
        for (int i = 0; i < values.Count; i++)
        {
            ///
            var item = new SerializedListNode<T>()
            {
                name = "data",
                type = typeId,
                val = values[i]
            };

            ///
            targetValueList.Add(item);
        }

        ///
        var str = UnityListPrefix + JsonUtility.ToJson(serializedListNode);

        ///
        GUIUtility.systemCopyBuffer = str;

        ///
        Debug.LogFormat("Converted list of {0} members of type {1}", values.Count, typeName);
    }

    private static bool GetJson(string unityList, out string json)
    {
        ///
        json = null;

        ///
        if (unityList.Length <= UnityListPrefix.Length)
        {
            ///
            return false;
        }

        ///
        if (unityList.Substring(0, UnityListPrefix.Length).ToUpper() != UnityListPrefix.ToUpper())
        {
            ///
            return false;
        }

        ///
        json = unityList.Substring(UnityListPrefix.Length);

        ///
        return true;
    }
}

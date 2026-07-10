using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MiniStorage
{
    [SerializeField]
    private StringIntSerializableDictionary intDictionary;
    [SerializeField]
    private StringFloatSerializableDictionary floatDictionary;
    [SerializeField]
    private StringDoubleSerializableDictionary doubleDictionary;
    [SerializeField]
    private StringStringSerializableDictionary stringDictionary;
    [SerializeField]
    private StringBoolSerializableDictionary boolDictionary;
    [SerializeField]
    private StringVector2SerializableDictionary vector2Dictionary;
    [SerializeField]
    private StringVector3SerializableDictionary vector3Dictionary;

    [System.NonSerialized]
    private HashSet<string> allKeys;
    [System.NonSerialized]
    private bool inited = false;

    private void TryInit()
    {
        ///
        if (inited)
        {
            return;
        }

        ///
        inited = true;

        ///
        allKeys = new HashSet<string>();

        ///
        AddKeysToSet(intDictionary);
        AddKeysToSet(floatDictionary);
        AddKeysToSet(stringDictionary);
        AddKeysToSet(boolDictionary);
        AddKeysToSet(vector2Dictionary);
        AddKeysToSet(vector3Dictionary);
    }

    private void AddKeysToSet<V>(Dictionary<string, V> dictionary)
    {
        ///
        if (dictionary == null)
        {
            return;
        }

        ///
        foreach (var key in dictionary.Keys)
        {
            allKeys.Add(key);
        }
    }

    public bool HasKey(string key)
    {
        ///
        TryInit();

        ///
        return allKeys.Contains(key);
    }

    private void SetValue<T, D>(string key, T value, ref D dictionary) where D : SerializableDictionary<string, T>, new()
    {
        ///
        TryInit();

        ///
        allKeys.Add(key);

        ///
        dictionary = dictionary ?? new D();
        dictionary[key] = value;
    }

    private T GetValue<T>(string key, T defaultValue, Dictionary<string, T> dictionary)
    {
        ///
        TryInit();

        ///
        if (dictionary == null)
        {
            return defaultValue;
        }

        ///
        T value;
        if (dictionary.TryGetValue(key, out value))
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }

    // int
    public void SetInt(string key, int value)
    {
        SetValue(key, value, ref intDictionary);
    }
    public int GetInt(string key, int defaultValue)
    {
        return GetValue(key, defaultValue, intDictionary);
    }

    // float
    public void SetFloat(string key, float value)
    {
        SetValue(key, value, ref floatDictionary);
    }
    public float GetFloat(string key, float defaultValue)
    {
        return GetValue(key, defaultValue, floatDictionary);
    }

    // string
    public void SetFloat(string key, string value)
    {
        SetValue(key, value, ref stringDictionary);
    }
    public string GetFloat(string key, string defaultValue)
    {
        return GetValue(key, defaultValue, stringDictionary);
    }

    // bool
    public void SetBool(string key, bool value)
    {
        SetValue(key, value, ref boolDictionary);
    }
    public bool GetBool(string key, bool defaultValue)
    {
        return GetValue(key, defaultValue, boolDictionary);
    }

    // Vector2
    public void SetVector2(string key, Vector2 value)
    {
        SetValue(key, value, ref vector2Dictionary);
    }
    public Vector2 GetVector2(string key, Vector2 defaultValue)
    {
        return GetValue(key, defaultValue, vector2Dictionary);
    }

    // Vector3
    public void SetVector3(string key, Vector3 value)
    {
        SetValue(key, value, ref vector3Dictionary);
    }
    public Vector3 GetVector3(string key, Vector3 defaultValue)
    {
        return GetValue(key, defaultValue, vector3Dictionary);
    }

    // double
    public void SetDouble(string key, double value)
    {
        SetValue(key, value, ref doubleDictionary);
    }
    public double GetDouble(string key, double defaultValue)
    {
        return GetValue(key, defaultValue, doubleDictionary);
    }
}

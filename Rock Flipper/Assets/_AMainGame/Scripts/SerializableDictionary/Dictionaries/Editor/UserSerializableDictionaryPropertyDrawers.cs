using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using SerializableDictionaryStorage;

[CustomPropertyDrawer(typeof(StringIntSerializableDictionary))]
[CustomPropertyDrawer(typeof(StringFloatSerializableDictionary))]
[CustomPropertyDrawer(typeof(StringDoubleSerializableDictionary))]
[CustomPropertyDrawer(typeof(StringBoolSerializableDictionary))]
[CustomPropertyDrawer(typeof(StringStringSerializableDictionary))]
[CustomPropertyDrawer(typeof(StringVector2SerializableDictionary))]
[CustomPropertyDrawer(typeof(StringVector3SerializableDictionary))]
[CustomPropertyDrawer(typeof(UISoundManager.UISoundSerializableDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

// [CustomPropertyDrawer(typeof(Vector3Storage))]
public class AnySerializableDictionaryStoragePropertyDrawer : SerializableDictionaryStoragePropertyDrawer { }

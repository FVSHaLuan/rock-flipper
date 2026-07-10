using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[System.Serializable]
public class StringIntSerializableDictionary : SerializableDictionary<string, int> { public StringIntSerializableDictionary() { } protected StringIntSerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { } };
[System.Serializable]
public class StringFloatSerializableDictionary : SerializableDictionary<string, float> { public StringFloatSerializableDictionary() { } protected StringFloatSerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { } };
[System.Serializable]
public class StringDoubleSerializableDictionary : SerializableDictionary<string, double> { public StringDoubleSerializableDictionary() { } protected StringDoubleSerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { } };
[System.Serializable]
public class StringBoolSerializableDictionary : SerializableDictionary<string, bool> { public StringBoolSerializableDictionary() { } protected StringBoolSerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { } };
[System.Serializable]
public class StringStringSerializableDictionary : SerializableDictionary<string, string> { public StringStringSerializableDictionary() { } protected StringStringSerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { } };
[System.Serializable]
public class StringVector2SerializableDictionary : SerializableDictionary<string, Vector2> { public StringVector2SerializableDictionary() { } protected StringVector2SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { } };
[System.Serializable]
public class StringVector3SerializableDictionary : SerializableDictionary<string, Vector3> { public StringVector3SerializableDictionary() { } protected StringVector3SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { } };
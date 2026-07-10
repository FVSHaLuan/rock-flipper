using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class DefaultValuesVerifier
{
    public static object GetDefaultValue(this Type type)
    {
        if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            return Activator.CreateInstance(type);
        else
            return null;
    }

    public static bool AreAllFieldsDefault<T>(T instance)
    {
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        ///
        foreach (var field in fields)
        {
            ///
            var defaultValue = GetDefaultValue(field.FieldType);
            var fieldValue = field.GetValue(instance);

            ///
            if (!IsEqual(defaultValue, fieldValue))
            {
                return false;
            };
        }

        ///
        return true;
    }

    private static bool IsEqual(object a, object b)
    {
        return a == null ? b == null : a.Equals(b);
    }
}

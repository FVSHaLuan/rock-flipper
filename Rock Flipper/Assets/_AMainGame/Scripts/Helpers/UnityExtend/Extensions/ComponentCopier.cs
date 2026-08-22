using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ComponentCopier
{
    public static T CopyToGameObject<T>(this T component, GameObject targetGameObject) where T : Component
    {
        ///
        System.Type type = component.GetType();
        T copy = targetGameObject.AddComponent(type) as T;

        // Copied fields can be restricted with BindingFlags
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(f => !f.IsNotSerialized);
        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(component));
        }

        ///
        return copy;
    }
}

using UnityEditor;
using UnityEngine;

public class TypedPlaceholderScript<T> : PlaceholderScript where T : Component
{
    protected override bool ValidateScriptReference(MonoScript scriptReference)
    {
        /// Check if the script reference is a subclass of T
        if (!scriptReference.GetClass().IsSubclassOf(typeof(T)))
        {
            Debug.LogWarning($"The referenced script is not a subclass of {typeof(T).Name}.");
            return false;
        }

        ///
        return base.ValidateScriptReference(scriptReference);
    }
}

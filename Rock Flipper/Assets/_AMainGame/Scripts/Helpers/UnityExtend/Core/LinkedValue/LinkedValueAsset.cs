using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LinkedValueAsset<T> : ScriptableObjectWithInit where T : struct
{
    public abstract T Value { get; }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        if (CheckForMutualRef(null))
        {
            Debug.LogError("Mutual references detected!");
        }
    }

    protected abstract bool CheckForMutualRefExternal(HashSet<ScriptableObject> previousObjects);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="previousObjects"></param>
    /// <returns>true if there IS mutual ref</returns>
    public bool CheckForMutualRef(HashSet<ScriptableObject> previousObjects)
    {
        ///
        if (previousObjects != null)
        {
            if (previousObjects.Contains(this))
            {
                return true;
            }
        }
        else
        {
            previousObjects = new HashSet<ScriptableObject>();
        }

        ///
        previousObjects.Add(this);

        ///
        return CheckForMutualRefExternal(previousObjects);
    }
#endif
}

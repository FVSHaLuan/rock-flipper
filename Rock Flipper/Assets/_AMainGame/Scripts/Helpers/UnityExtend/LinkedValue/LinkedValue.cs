using OneLine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LinkedValue<T> : IOneLineAddedLabel where T : struct
{
    [System.NonSerialized]
    private T cachedValue;
    [System.NonSerialized]
    private bool hasCachedValue;

    public T Value
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return GetValue();
            }
#endif

            if (!hasCachedValue)
            {
                ///
#if UNITY_EDITOR
                if (CheckForCheckForMutualRefExternal(null))
                {
                    throw new System.OverflowException("LinkedValue has mutual references!");
                }
#endif

                ///
                cachedValue = GetValue();
            }

            return cachedValue;
        }
    }

    public virtual string AddedLabel => Value.ToString();

    protected abstract T GetValue();

#if UNITY_EDITOR
    public abstract bool CheckForCheckForMutualRefExternal(HashSet<ScriptableObject> previousObjects); 
#endif
}

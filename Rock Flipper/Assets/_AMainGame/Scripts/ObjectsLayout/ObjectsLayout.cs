using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectsLayout : MonoBehaviour, IEnumerable<ObjectLayoutInfo>
{
    public abstract IEnumerator<ObjectLayoutInfo> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

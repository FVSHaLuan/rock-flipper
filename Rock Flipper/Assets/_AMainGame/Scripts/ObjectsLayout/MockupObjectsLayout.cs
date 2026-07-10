using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockupObjectsLayout : ObjectsLayout
{
    public override IEnumerator<ObjectLayoutInfo> GetEnumerator()
    {
        yield return new ObjectLayoutInfo()
        {
            position = transform.position,
            normalizedTangent = Vector3.up
        };
    }
}

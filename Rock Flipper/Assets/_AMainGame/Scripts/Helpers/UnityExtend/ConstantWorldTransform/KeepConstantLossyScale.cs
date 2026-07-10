using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepConstantLossyScale : MonoBehaviour
{
    [SerializeField]
    private Vector3 lossyScale = Vector3.one;

    protected void LateUpdate()
    {
        var currentLossyScale = transform.lossyScale;
        var currentLocalScale = transform.localScale;

        ///
        Vector3 localScale = new Vector3()
        {
            x = lossyScale.x / currentLossyScale.x * currentLocalScale.x,
            y = lossyScale.y / currentLossyScale.y * currentLocalScale.y,
            z = lossyScale.z / currentLossyScale.z * currentLocalScale.z,
        };

        ///
        if (float.IsInfinity(localScale.x) || float.IsInfinity(localScale.y) || float.IsInfinity(localScale.z))
        {
            return;
        }

        ///
        transform.localScale = localScale;
    }
}

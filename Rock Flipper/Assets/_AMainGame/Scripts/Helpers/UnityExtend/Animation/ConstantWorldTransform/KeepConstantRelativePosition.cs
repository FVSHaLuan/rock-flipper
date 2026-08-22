using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepConstantRelativePosition : MonoBehaviour
{
    [SerializeField]
    private Transform referencedTransform;
    [SerializeField]
    private Vector3 relativePosition;

    protected void LateUpdate()
    {
        transform.position = referencedTransform.position + relativePosition;
    }
}

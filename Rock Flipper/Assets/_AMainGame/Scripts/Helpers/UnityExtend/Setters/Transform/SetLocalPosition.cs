using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLocalPosition : MonoBehaviour
{
    [SerializeField]
    private Vector3 localPosition;

    public void Set()
    {
        transform.localPosition = localPosition;
    }
}

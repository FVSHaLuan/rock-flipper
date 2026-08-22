using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWorldPosition : MonoBehaviour
{
    [SerializeField]
    private Vector3 worldPosition;

    public void Set()
    {
        transform.position = worldPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepConstantUpVector : MonoBehaviour
{
    [SerializeField]
    private Vector3 up = Vector3.up;

    public void LateUpdate()
    {
        transform.up = up;
    }
}

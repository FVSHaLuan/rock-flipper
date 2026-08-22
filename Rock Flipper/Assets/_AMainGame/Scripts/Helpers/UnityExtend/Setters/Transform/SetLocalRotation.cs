using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLocalRotation : MonoBehaviour
{
    [SerializeField]
    private Vector3 eulerAngles;

    public void Set()
    {
        transform.localEulerAngles = eulerAngles;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLocalScale : MonoBehaviour
{
    [SerializeField]
    Vector3 localScale = Vector3.one;

    public void Set()
    {
        transform.localScale = localScale;
    }

    public void SetUniformScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }
}

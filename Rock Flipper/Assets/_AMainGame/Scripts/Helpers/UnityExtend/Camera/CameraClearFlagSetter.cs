using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraClearFlagSetter : MonoBehaviour
{
    public void SetDontClear()
    {
        GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
    }

    public void SetSolidColor()
    {
        GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
    }
}

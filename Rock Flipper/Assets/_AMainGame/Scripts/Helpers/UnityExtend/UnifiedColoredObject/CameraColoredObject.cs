using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraColoredObject : UnifiedColoredObject
{
    new private Camera camera;

    protected override void SetColor(Color color)
    {
        ///
        if (camera == null)
        {
            camera = GetComponent<Camera>();
        }

        ///
        camera.backgroundColor = color;
    }
}

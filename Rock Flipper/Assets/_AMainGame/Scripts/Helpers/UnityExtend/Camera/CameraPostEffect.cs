using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPostEffect : MonoBehaviour
{
    [SerializeField]
    private Material material;

    protected void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}

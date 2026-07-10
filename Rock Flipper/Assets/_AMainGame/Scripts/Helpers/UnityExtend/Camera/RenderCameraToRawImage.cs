using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class RenderCameraToRawImage : MonoBehaviour
{
    [SerializeField]
    private RenderTextureFormat renderTextureFormat;
    [SerializeField]
    private int depth = 32;

    [Space]
    [SerializeField]
    private RawImage rawImage;

    private RenderTexture renderTexture;

    protected void Awake()
    {
        ///
        renderTexture = new RenderTexture(Screen.width, Screen.height, depth, renderTextureFormat);
        GetComponent<Camera>().targetTexture = renderTexture;
        rawImage.texture = renderTexture;
    }
}

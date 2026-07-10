using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderCameraToRenderer : MonoBehaviour
{
    [SerializeField]
    private RenderTextureFormat renderTextureFormat;
    [SerializeField]
    private int depth = 32;
    [SerializeField]
    private Vector2Int customResolution;

    [Space]
    [SerializeField]
    private new Renderer renderer;

    [Space]
    [SerializeField]
    private bool releaseTextureOnDisable = true;
    [SerializeField]
    private bool releaseTextureOnDestroy = true;

    private RenderTexture renderTexture;

    protected void Awake()
    {
        ///
        if (customResolution.x <= 0 || customResolution.y <= 0)
        {
            renderTexture = new RenderTexture(Screen.width, Screen.height, depth, renderTextureFormat);
        }
        else
        {
            renderTexture = new RenderTexture(customResolution.x, customResolution.y, depth, renderTextureFormat);
        }

        ///
        GetComponent<Camera>().targetTexture = renderTexture;

        ///
        renderer.material.mainTexture = renderTexture;
    }

    protected void OnDisable()
    {
        if (releaseTextureOnDisable)
        {
            TryReleaseRenderTexture();
        }
    }

    protected void OnDestroy()
    {
        if (releaseTextureOnDestroy)
        {
            TryReleaseRenderTexture();
        }
    }

    private void TryReleaseRenderTexture()
    {
        ///
        if (renderTexture == null)
        {
            return;
        }

        ///
        renderTexture.Release();
        renderTexture.DiscardContents();
        Destroy(renderTexture);

        ///
        renderTexture = null;
    }
}

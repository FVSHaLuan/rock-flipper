using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(RawImage), typeof(VideoPlayer))]
public class FullscreenVideoPlayer : MonoBehaviourWithInit
{
    [SerializeField]
    private float maxTextureSize = 1920;
    [SerializeField]
    private RenderTextureFormat renderTextureFormat = RenderTextureFormat.RGB565;
    [SerializeField]
    private bool destroyRenderTextureOnDisabled = true;

    private RenderTexture renderTexture;
    private RawImage rawImage;
    public VideoPlayer VideoPlayer { get; private set; }

    protected override bool Init()
    {
        ///
        rawImage = GetComponent<RawImage>();

        ///
        VideoPlayer = GetComponent<VideoPlayer>();
        VideoPlayer.renderMode = VideoRenderMode.RenderTexture;
        VideoPlayer.aspectRatio = VideoAspectRatio.FitInside;

        ///
        return base.Init();
    }

    private void CreateRenderTexture()
    {
        ///
        // Screen.width, Screen.height: return active Unity's sub window (inspector, hierarchy,...)
        Vector2Int size = new Vector2Int(Screen.width, Screen.height);

        ///
        float scaleFactor = 1;
        var maxSize = Mathf.Max(size.x, size.y);
        if (maxSize > maxTextureSize)
        {
            scaleFactor = maxTextureSize / maxSize;
        }

        ///
        size.x = Mathf.FloorToInt(scaleFactor * size.x);
        size.y = Mathf.FloorToInt(scaleFactor * size.y);

        ///
        renderTexture = new RenderTexture(size.x, size.y, 24, renderTextureFormat);

        ///
        rawImage.texture = renderTexture;
        VideoPlayer.targetTexture = renderTexture;
    }

    protected virtual void OnEnable()
    {
        ///
        if (renderTexture==null)
        {
            CreateRenderTexture();
        }
    }

    protected void OnDisable()
    {
        ///
        if (destroyRenderTextureOnDisabled)
        {
            renderTexture.Release();
            Destroy(renderTexture);
        }
    }
}

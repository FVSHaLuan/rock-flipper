using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectLimiter : MonoBehaviour
{
    private Camera _camera;

    protected void OnDisable()
    {
        ScreenSizeChangeDetector.DirectInstance.OnScreenSizeChanged -= ScreenSizeChangeDetector_OnScreenSizeChanged;
    }

    protected void OnEnable()
    {
        Limit();
        ScreenSizeChangeDetector.Instance.OnScreenSizeChanged += ScreenSizeChangeDetector_OnScreenSizeChanged;
    }

    private void ScreenSizeChangeDetector_OnScreenSizeChanged()
    {
        Limit();
    }

    private void Limit()
    {
        ///
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
        }
        var cam = _camera;

        ///
        float windowAspect = (float)Screen.width / Screen.height;
        var minAspect = ScreenSizeConfig.MinWidthRatio;
        var maxAspect = ScreenSizeConfig.MaxWidthRatio;

        if (windowAspect < minAspect)
        {
            // Too tall -> letterbox
            float scaleHeight = windowAspect / minAspect;
            cam.rect = new Rect(
                0f,
                (1f - scaleHeight) / 2f,
                1f,
                scaleHeight
            );
        }
        else if (windowAspect > maxAspect)
        {
            // Too wide -> pillarbox
            float scaleWidth = maxAspect / windowAspect;
            cam.rect = new Rect(
                (1f - scaleWidth) / 2f,
                0f,
                scaleWidth,
                1f
            );
        }
        else
        {
            // Within acceptable range -> full screen
            cam.rect = new Rect(0f, 0f, 1f, 1f);
        }
    }
}

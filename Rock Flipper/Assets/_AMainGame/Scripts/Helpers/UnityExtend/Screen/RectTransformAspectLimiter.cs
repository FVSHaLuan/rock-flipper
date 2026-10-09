using UnityEngine;

/// <summary>
/// Same as <see cref="CameraAspectLimiter"/>, but for a <see cref="RectTransform"/> that is the immediate child of a Canvas.
/// Letterboxes/pillarboxes the RectTransform (via its anchors) so its aspect stays within the configured range.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class RectTransformAspectLimiter : MonoBehaviour
{
    private RectTransform _rectTransform;

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
        if (_rectTransform == null)
        {
            _rectTransform = (RectTransform)transform;
        }
        var rt = _rectTransform;

        ///
        float windowAspect = (float)Screen.width / Screen.height;
        var minAspect = ScreenSizeConfig.MinWidthRatio;
        var maxAspect = ScreenSizeConfig.MaxWidthRatio;

        if (windowAspect < minAspect)
        {
            // Too tall -> letterbox
            float scaleHeight = windowAspect / minAspect;
            SetNormalizedRect(rt, 0f, (1f - scaleHeight) / 2f, 1f, scaleHeight);
        }
        else if (windowAspect > maxAspect)
        {
            // Too wide -> pillarbox
            float scaleWidth = maxAspect / windowAspect;
            SetNormalizedRect(rt, (1f - scaleWidth) / 2f, 0f, scaleWidth, 1f);
        }
        else
        {
            // Within acceptable range -> full screen
            SetNormalizedRect(rt, 0f, 0f, 1f, 1f);
        }
    }

    private static void SetNormalizedRect(RectTransform rt, float x, float y, float width, float height)
    {
        rt.anchorMin = new Vector2(x, y);
        rt.anchorMax = new Vector2(x + width, y + height);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}

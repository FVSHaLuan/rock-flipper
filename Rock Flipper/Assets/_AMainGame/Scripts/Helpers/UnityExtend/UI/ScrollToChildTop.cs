using UnityEngine;
using UnityEngine.UI;

public class ScrollToChildTop : MonoBehaviour
{
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private RectTransform targetChild; // Immediate child of scrollRect.content
    [SerializeField]
    private float padding = 0f;

    private Vector3[] targetWorldCorners = new Vector3[4];
    private Vector3[] contentWorldCorners = new Vector3[4];

    public RectTransform TargetChild { get => targetChild; set => targetChild = value; }

    /// <summary>
    /// Scrolls the ScrollRect so that the target is visible at the top.
    /// </summary>
    [ContextMenu("Scroll To Target Top")]
    public void ScrollToTop()
    {
        if (scrollRect == null)
        {
            scrollRect = GetComponentInParent<ScrollRect>();

            ///
            if (scrollRect == null)
            {
                Debug.LogWarning("ScrollRect not assigned and not found!");
                return;
            }
        }

        if (TargetChild == null)
        {
            TargetChild = GetComponent<RectTransform>();

            ///
            if (TargetChild == null)
            {
                Debug.LogWarning("TargetChild not assigned and not found!");
                return;
            }
        }

        ///
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();

        // Make sure layouts are up to date
        Canvas.ForceUpdateCanvases();

        // --- Convert target’s top edge to content local space ---

        TargetChild.GetWorldCorners(targetWorldCorners);
        // Corners order: bottom-left, top-left, top-right, bottom-right
        Vector3 targetTopWorld = targetWorldCorners[1];
        content.GetWorldCorners(contentWorldCorners);
        Vector3 contentTopWorld = contentWorldCorners[1];
        Vector3 contentBottomWorld = contentWorldCorners[0];

        // Convert world positions to local positions within the content
        Vector3 targetTopLocal = content.InverseTransformPoint(targetTopWorld);
        Vector3 contentTopLocal = content.InverseTransformPoint(contentTopWorld);
        Vector3 contentBottomLocal = content.InverseTransformPoint(contentBottomWorld);

        float contentHeight = contentTopLocal.y - contentBottomLocal.y;
        float viewportHeight = viewport.rect.height;

        // Distance from content top to target top in local space
        float distanceFromTop = contentTopLocal.y - targetTopLocal.y - padding;

        // Normalized scroll position: 1 = top, 0 = bottom
        float normalized = 1f - Mathf.Clamp01(distanceFromTop / (contentHeight - viewportHeight));

        scrollRect.verticalNormalizedPosition = normalized;
    }
}

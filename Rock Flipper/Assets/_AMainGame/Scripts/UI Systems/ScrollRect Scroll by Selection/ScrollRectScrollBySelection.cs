using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using BT.FeatureBranching;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectScrollBySelection : ExtendedMonoBehaviour
{
    [Space]
    [SerializeField]
    private bool scrollX = true;
    [SerializeField]
    private bool scrollY = true;

    [Space]
    [SerializeField]
    private RectTransform overrideViewPort;

    [Space]
    [SerializeField]
    private bool updatePositionOnEnable = true;

    private RectTransform rectTransformToScroll;
    private RectTransform viewPort;
    private Vector2 targetPos;

    // It starts bottom left and rotates to top left, then top right, and finally bottom right. Note that bottom left, for example, is an (x, y, z) vector with x being left and y being bottom.
    // https://docs.unity3d.com/ScriptReference/RectTransform.GetWorldCorners.html
    Vector3[] refWorldCorners = new Vector3[4];
    Vector3[] viewWorldCorners = new Vector3[4];

    private bool TryGetRectTransforms()
    {
        ///
        if (rectTransformToScroll != null && viewPort != null)
        {
            return true;
        }

        ///
        var scrollRect = GetComponent<ScrollRect>();
        rectTransformToScroll = scrollRect.content;
        if (overrideViewPort == null)
        {
            viewPort = scrollRect.viewport;
        }
        else
        {
            viewPort = overrideViewPort;
        }

        ///
        if (rectTransformToScroll != null && viewPort != null)
        {
            return true;
        }

        ///
        EnhancedScroller enhancedScroller = GetComponent<EnhancedScroller>();
        if (enhancedScroller != null)
        {
            rectTransformToScroll = enhancedScroller.Container;
            if (overrideViewPort == null)
            {
                viewPort = GetComponent<RectTransform>();
            }
            else
            {
                viewPort = overrideViewPort;
            }
        }

        ///
        if (rectTransformToScroll != null && viewPort != null)
        {
            return true;
        }

        ///
        return false;
    }

    public void OnDisable()
    {
        ///
        StopAllCoroutines();

        ///
        entry.uiSelectedEventManager.OnSelectionChanged -= UiSelectedEventManager_OnSelectionChanged;
    }

    public void OnEnable()
    {
        ///
        if (updatePositionOnEnable)
        {
            StartCoroutine(UpdateScrollingTargetMultipleTimes());
        }

        ///
        entry.uiSelectedEventManager.OnSelectionChanged += UiSelectedEventManager_OnSelectionChanged;
    }

    private IEnumerator UpdateScrollingTargetMultipleTimes()
    {
        UpdateScrollingTarget();
        yield return new WaitForEndOfFrame();
        UpdateScrollingTarget();
        yield return new WaitForEndOfFrame();
        UpdateScrollingTarget();
        yield return new WaitForEndOfFrame();
    }

    private void UiSelectedEventManager_OnSelectionChanged()
    {
        UpdateScrollingTarget();
    }

    [ContextMenu("UpdateScrollingTarget")]
    public void UpdateScrollingTarget()
    {
        ///
        if (!TryGetRectTransforms())
        {
            return;
        };

        ///
        RectTransform refRectTransform = null;
        ScrollRectSelectionSnapItem refItem;

        ///
        var currentSelectedGameObject = entry.uiSelectedEventManager.LastGameObject;
        if (!IsSnappable(currentSelectedGameObject, out refItem, out refRectTransform))
        {
            return;
        }

        ///
        LogFormat("Update with refItem = {0}", refRectTransform.gameObject.name);

        ///
        refRectTransform.GetWorldCorners(refWorldCorners);
        viewPort.GetWorldCorners(viewWorldCorners);

        ///
        var refRect = new Rect(refWorldCorners[0], refWorldCorners[2] - refWorldCorners[0]);
        var viewRect = new Rect(viewWorldCorners[0], viewWorldCorners[2] - viewWorldCorners[0]);

        // Already partially visible
        if (refRect.Overlaps(viewRect)
            &&
            (
            entry.inputManager.ActiveSimplifiedInputDevice.deviceType == SimplifiedInputDeviceType.MouseAndKeyboard
            || VersionBranchInfo.IsTargetedOrOnMobile
            )
            )
        {
            return;
        }

        // Calculate displacement of refRect's bottom left
        //---
        var minX = viewRect.xMin;
        var maxX = viewRect.xMax - refRect.width;
        var minY = viewRect.yMin;
        var maxY = viewRect.yMax - refRect.height;
        //---
        var currentRefRectBottomLeft = refWorldCorners[0];
        var newRefRectBottomLeft = currentRefRectBottomLeft;
        if (scrollX)
        {
            newRefRectBottomLeft.x = Mathf.Clamp(newRefRectBottomLeft.x, minX, maxX);
        }
        if (scrollY)
        {
            newRefRectBottomLeft.y = Mathf.Clamp(newRefRectBottomLeft.y, minY, maxY);
        }
        //---
        var displacementVector = newRefRectBottomLeft - currentRefRectBottomLeft;

        ///
        rectTransformToScroll.position += displacementVector;

        ///
        var refSelectable = entry.uiSelectedEventManager.LastSelectable;
        if (refSelectable != null)
        {
            var scrollRect = GetComponent<ScrollRect>();
            // Top
            if (IsTopMost(refSelectable))
            {
                scrollRect.verticalNormalizedPosition = 1;
            }
            // Bottom
            else if (IsBottomMost(refSelectable))
            {
                scrollRect.verticalNormalizedPosition = 0;
            }
        }
    }

    private bool IsTopMost(Selectable selectable)
    {
        var rs = selectable.FindSelectableOnUp();

        ///
        if (rs == null)
        {
            return true;
        }

        ///
        return !IsSnappable(rs.gameObject, out _, out _);
    }

    private bool IsBottomMost(Selectable selectable)
    {
        var rs = selectable.FindSelectableOnDown();

        ///
        if (rs == null)
        {
            return true;
        }

        ///
        return !IsSnappable(rs.gameObject, out _, out _);
    }

    private bool IsSnappable(GameObject selectedGameObject, out ScrollRectSelectionSnapItem refItem, out RectTransform refRectTransform)
    {
        ///
        if (selectedGameObject == null)
        {
            refItem = null;
            refRectTransform = null;
            return false;
        }

        ///
        refItem = selectedGameObject.GetComponentInParent<ScrollRectSelectionSnapItem>();

        ///
        if (refItem == null || !refItem.transform.IsChildOf(rectTransformToScroll))
        {
            refRectTransform = null;
            return false;
        }
        else
        {
            refRectTransform = refItem.RefRectTransform;
            if (refRectTransform == null)
            {
                return false;
            }
        }

        ///
        return true;
    }

    public void UpdateScrollingTargetAtEndOfFrame()
    {
        ///
        if (!gameObject.activeInHierarchy)
        {
            ///
            Log("Couldn't UpdateScrollingTargetAtEndOfFrame because !gameObject.activeInHierarchy");

            ///
            return;
        }

        ///
        StopAllCoroutines();

        ///
        StartCoroutine(UpdateScrollingTargetAtEndOfFrameCoroutine());
    }

    private IEnumerator UpdateScrollingTargetAtEndOfFrameCoroutine()
    {
        ///
        yield return new WaitForEndOfFrame();

        ///
        UpdateScrollingTarget();
    }
}

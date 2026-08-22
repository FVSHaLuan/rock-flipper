using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectExtensions
{
    private static Vector3[] viewportCorners = new Vector3[4];
    private static Vector3[] contentCorners = new Vector3[4];

    public static Vector3 GetVerticallyCenteredContentPosition(this ScrollRect scrollRect, Transform child, Transform centerPoint)
    {
        ///
        Vector3 centreWorldPos;

        ///
        if (centerPoint == null)
        {
            RectTransform viewport = scrollRect.viewport;
            viewport = viewport != null ? viewport : (RectTransform)scrollRect.transform;
            viewport.GetWorldCorners(viewportCorners);

            ///
            centreWorldPos = ((viewportCorners[1] - viewportCorners[0]) / 2f) + viewportCorners[0];
        }
        else
        {
            centreWorldPos = centerPoint.position;
        }

        ///
        float h = centreWorldPos.y - child.position.y;
        Vector3 displacement = new Vector3(0, h, 0);

        ///
        scrollRect.content.GetWorldCorners(contentCorners);

        ///
        if (contentCorners[1].y + displacement.y < viewportCorners[1].y)
        {
            displacement.y = viewportCorners[1].y - contentCorners[1].y;
        }
        else if (contentCorners[0].y + displacement.y > viewportCorners[0].y)
        {
            displacement.y = viewportCorners[0].y - contentCorners[0].y;
        }

        ///
        return scrollRect.content.position + displacement;
    }
}

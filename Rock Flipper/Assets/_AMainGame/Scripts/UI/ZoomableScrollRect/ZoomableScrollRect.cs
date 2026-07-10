using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoomableScrollRect : ScrollRect
{
    public Vector2 ContinuousNormalizedPosition
    {
        get
        {
            return new Vector2(ContinuousHorizontalNormalizedPosition, ContinuousVerticalNormalizedPosition);
        }
    }

    public float ContinuousHorizontalNormalizedPosition
    {
        get
        {
            UpdateBounds();
            var m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
            if ((m_ContentBounds.size.x <= m_ViewBounds.size.x) || Mathf.Approximately(m_ContentBounds.size.x, m_ViewBounds.size.x))
                return 0;
            return (m_ViewBounds.min.x - m_ContentBounds.min.x) / (m_ContentBounds.size.x - m_ViewBounds.size.x);
        }
    }

    public float ContinuousVerticalNormalizedPosition
    {
        get
        {
            UpdateBounds();
            var m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
            if ((m_ContentBounds.size.y <= m_ViewBounds.size.y) || Mathf.Approximately(m_ContentBounds.size.y, m_ViewBounds.size.y))
                return 0;

            return (m_ViewBounds.min.y - m_ContentBounds.min.y) / (m_ContentBounds.size.y - m_ViewBounds.size.y);
        }
        set
        {
            SetNormalizedPosition(value, 1);
        }
    }

    public override void OnScroll(PointerEventData data)
    {
        // base.OnScroll(data);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        eventData.button = PointerEventData.InputButton.Left;

        ///
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        eventData.button = PointerEventData.InputButton.Left;

        ///
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        eventData.button = PointerEventData.InputButton.Left;

        ///
        base.OnEndDrag(eventData);
    }
}

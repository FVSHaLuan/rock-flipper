using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
[DisallowMultipleComponent]
public class ZoomableScrollRectController : MonoBehaviourWithInit
{
    [SerializeField, Min(0.001f)]
    private float minZoom = 1;
    [SerializeField, Min(0.001f)]
    private float maxZoom = 3;

    [Space]
    [SerializeField]
    private float overtimeZoomingSpeed = 1.0f;

    [Space]
    [SerializeField, Min(0)]
    private float buttonZoomStep = 0.1f;

    private ScrollRect scrollRect;
    private Canvas canvas;
    private float targetZoom;
    private Vector2 targetLocalPos;
    private bool isZooming = false;

    protected override bool Init()
    {
        ///
        scrollRect = GetComponent<ScrollRect>();
        canvas = GetComponentInParent<Canvas>(true);

        ///
        return base.Init();
    }

    protected void OnDisable()
    {
        if (isZooming)
        {
            SetZoomByTargetLocalPositionImmediately(targetZoom, targetLocalPos);
            isZooming = false;
        }
    }

    protected void Update()
    {
        UpdateZoomByMouse();
        ZoomOverTime();
    }

    private void ZoomOverTime()
    {
        if (!isZooming)
        {
            return;
        }

        ///
        var currentZoom = scrollRect.content.localScale.x;

        ///
        var zoom = Mathf.MoveTowards(currentZoom, targetZoom, overtimeZoomingSpeed * Time.deltaTime);
        SetZoomByTargetLocalPositionImmediately(zoom, targetLocalPos);

        ///
        if (Mathf.Approximately(currentZoom, targetZoom))
        {
            isZooming = false;
        }
    }

    private void UpdateZoomByMouse()
    {
        float scrollWheel = Mouse.current.scroll.ReadValue().y;

        ///
        if (scrollWheel == 0)
        {
            return;
        }

        ///
        Vector2 inputPosition = Mouse.current.position.ReadValue();

        // Return if the mouse is outside the game's window
        if (inputPosition.x < 0 || inputPosition.x > Screen.width || inputPosition.y < 0 || inputPosition.y > Screen.height)
        {
            return;
        }

        ///
        if (scrollWheel < 0)
        {
            ButtonZoomIn(inputPosition);
        }
        else if (scrollWheel > 0)
        {
            ButtonZoomOut(inputPosition);
        }
    }

    public void ButtonZoomIn(Vector2 targetScreenPoint)
    {
        Zoom(-buttonZoomStep, targetScreenPoint);
    }

    public void ButtonZoomOut(Vector2 targetScreenPoint)
    {
        Zoom(buttonZoomStep, targetScreenPoint);
    }

    public void Zoom(float deltaZoom, Vector2 targetScreenPoint)
    {
        var zoom = scrollRect.content.localScale.x + deltaZoom;
        SetZoomOvertime(zoom, targetScreenPoint);
    }

    public void SetZoomOvertime(float zoom, Vector2 targetScreenPoint)
    {
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        ///
        Vector2 targetLocalPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect.content, targetScreenPoint, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out targetLocalPos);

        ///
        targetZoom = zoom;
        this.targetLocalPos = targetLocalPos;
        isZooming = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="zoom"></param>
    /// <param name="targetLocalPos">local position relative to the content</param>
    private void SetZoomByTargetLocalPositionImmediately(float zoom, Vector2 targetLocalPos)
    {
        var savedZoom = scrollRect.content.localScale.x;
        scrollRect.content.localScale = Vector3.one * zoom;

        ///
        var displacement = targetLocalPos * zoom - targetLocalPos * savedZoom;
        scrollRect.content.anchoredPosition -= displacement;
    }
}

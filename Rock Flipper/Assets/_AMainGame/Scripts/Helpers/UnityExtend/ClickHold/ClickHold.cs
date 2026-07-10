using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClickHold : MonoBehaviourWithInit, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerExitHandler, IPointerEnterHandler
{
    public static event System.Action<ClickHold> OnAnyClickHoldStarted;
    public static event System.Action<ClickHold> OnAnyClickHoldCancelled;

    public static IHoverMaxBuyService HoverBuyService { get; set; }

    [SerializeField]
    private float threshold = 0.2f; // Time in seconds to trigger hold action
    [SerializeField]
    private float initialClickSpeed = 10f; // Initial speed of action when hold is triggered, times per sec
    [SerializeField]
    private float maxClickSpeed = 100f; // Max speed of action when hold is triggered, times per sec
    [SerializeField]
    private float clickAcceleration = 10f; // Acceleration of action speed, times per sec^2
    [SerializeField]
    private int buyMaxCount = 9999;

    [Space]
    [SerializeField]
    private UnityEvent<int> onHoldClick;
    [SerializeField]
    private UnityEvent clickSoundDelegate;
    [SerializeField]
    private UnityEvent clickParticleDelegate;

    private bool isHolding = false;
    private float holdStartTime = 0f;
    private float timeAccount = 0f;
    private bool passedThreshold = false;
    private int totalClickSinceHoldStart = 0;
    private bool isMouseHovering = false;
    private Selectable selectable;

    protected override bool Init()
    {
        ///
        selectable = GetComponent<Selectable>();

        ///
        return base.Init();
    }

    protected void OnDisable()
    {
        isHolding = false;
        isMouseHovering = false;
        Cancel();
    }

    protected void Update()
    {
        ///
        if (HoverBuyService != null
            && HoverBuyService.IsHoldingTheKey
            && isMouseHovering
            && selectable != null
            && selectable.IsInteractable()
            )
        {
            onHoldClick?.Invoke(buyMaxCount);
            clickParticleDelegate?.Invoke();
            clickSoundDelegate?.Invoke();
            return;
        }


        ///
        if (!isHolding)
        {
            return;
        }

        ///
        if (selectable != null && !selectable.IsInteractable())
        {
            Cancel();
            return;
        }

        ///
        float timeSincePassedThreshold = Time.unscaledTime - holdStartTime - threshold;
        bool passedThresholdThisFrame = false;

        ///
        if (!passedThreshold
            && timeSincePassedThreshold >= 0)
        {
            passedThreshold = true;
            timeAccount = timeSincePassedThreshold;
            passedThresholdThisFrame = true;
        }
        else
        {
            timeAccount += Time.unscaledDeltaTime;
        }

        ///
        if (!passedThreshold)
        {
            return;
        }

        ///
        if (passedThresholdThisFrame)
        {
            OnAnyClickHoldStarted?.Invoke(this);
        }

        ///
        var speed = Mathf.Min(maxClickSpeed, initialClickSpeed + timeSincePassedThreshold * clickAcceleration);
        var interval = 1f / speed;
        // Debug.Log($"speed {speed}, interval {interval}, timeAccount {timeAccount}");
        int clickCount = 0;
        while (timeAccount >= interval)
        {
            timeAccount -= interval;
            clickCount++;
        }

        ///
        totalClickSinceHoldStart += clickCount;

        ///
        bool isHoldingBuyMax = IsHoldingBuyMax();
        if (isHoldingBuyMax)
        {
            clickCount = buyMaxCount;
        }

        ///
        if (clickCount > 0)
        {
            onHoldClick?.Invoke(clickCount);

            ///
            clickParticleDelegate?.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ///
        if (selectable == null || !selectable.IsInteractable())
        {
            return;
        }

        ///
        if (!isHolding)
        {
            isHolding = true;
            holdStartTime = Time.unscaledTime;
            passedThreshold = false;
            totalClickSinceHoldStart = 0;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ///
        if (selectable == null || !selectable.IsInteractable())
        {
            return;
        }

        ///
        if (totalClickSinceHoldStart == 0)
        {
            if (IsHoldingBuyMax())
            {
                onHoldClick?.Invoke(buyMaxCount);
            }
            else
            {
                onHoldClick?.Invoke(1);
            }
        }

        ///
        Cancel();
    }

    private void Cancel()
    {
        isHolding = false;
        timeAccount = 0f;
        passedThreshold = false;
        totalClickSinceHoldStart = 0;

        ///
        OnAnyClickHoldCancelled?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isHolding)
        {
            eventData.pointerDrag = null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseHovering = false;

        ///
        Cancel();
    }

    private bool IsHoldingBuyMax()
    {
        return Keyboard.current.ctrlKey.isPressed;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseHovering = true;
    }
}

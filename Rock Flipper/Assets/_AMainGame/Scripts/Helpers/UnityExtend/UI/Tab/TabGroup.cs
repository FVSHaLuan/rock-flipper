using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TabGroup : MonoBehaviourWithInit
{
    [SerializeField]
    private TabButton defaultTab;

    [Header("Active Indicator")]
    [SerializeField, Tooltip("Optional. Slides horizontally to the active tab button. Should share the tab buttons' parent and be ignored by its layout.")]
    private RectTransform activeIndicator;
    [SerializeField, Min(0)]
    private float indicatorMoveDuration = 0.4f;
    [SerializeField, Tooltip("The indicator's progress from its start to the active tab button over the move's duration (0 to 1). Values above 1 overshoot the button. Should end at 1.")]
    private AnimationCurve indicatorMoveCurve = new AnimationCurve(
        new Keyframe(0f, 0f, 0f, 5f),
        new Keyframe(0.35f, 1.12f, 0f, 0f),
        new Keyframe(0.6f, 0.96f, 0f, 0f),
        new Keyframe(0.82f, 1.015f, 0f, 0f),
        new Keyframe(1f, 1f, 0f, 0f));
    [SerializeField]
    private bool indicatorUsesUnscaledTime = true;

    private List<TabButton> tabButtons;
    private TabButton activeTab;

    private bool hasPlacedIndicator;
    private float indicatorStartX;
    private float indicatorMoveElapsed;

    protected override bool Init()
    {
        tabButtons = new List<TabButton>();
        GetComponentsInChildren(tabButtons);

        ///
        SetActiveTab(defaultTab);

        ///
        return base.Init();
    }

    public void SetActiveTab(TabButton tabButton)
    {
        ///
        if (tabButton != null && tabButton.IsLocked)
        {
            return;
        }

        ///
        foreach (var button in tabButtons)
        {
            button.SetActive(button == tabButton);
        }

        ///
        if (tabButton != activeTab && activeIndicator != null && hasPlacedIndicator)
        {
            indicatorStartX = activeIndicator.localPosition.x;
            indicatorMoveElapsed = 0f;
        }
        activeTab = tabButton;
    }

    /// <summary>
    /// Runs every frame rather than only on tab changes, so the indicator also follows its tab button when the layout
    /// moves the buttons (e.g. on the first layout pass or a screen resize).
    /// </summary>
    protected void LateUpdate()
    {
        ///
        if (activeIndicator == null || activeTab == null)
        {
            return;
        }

        ///
        var targetX = GetIndicatorX(activeTab);

        // The first placement is immediate
        if (!hasPlacedIndicator)
        {
            hasPlacedIndicator = true;
            indicatorMoveElapsed = indicatorMoveDuration;
        }

        ///
        float x;
        if (indicatorMoveElapsed < indicatorMoveDuration)
        {
            indicatorMoveElapsed += indicatorUsesUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            var progress = indicatorMoveCurve.Evaluate(Mathf.Clamp01(indicatorMoveElapsed / indicatorMoveDuration));
            x = Mathf.LerpUnclamped(indicatorStartX, targetX, progress);
        }
        else
        {
            x = targetX;
        }

        ///
        var position = activeIndicator.localPosition;
        position.x = x;
        activeIndicator.localPosition = position;
    }

    /// <summary>
    /// The indicator's local x that centers it horizontally on the given tab button, whatever their pivots.
    /// </summary>
    private float GetIndicatorX(TabButton tabButton)
    {
        var buttonTransform = (RectTransform)tabButton.transform;
        var buttonCenterX = buttonTransform.localPosition.x + buttonTransform.rect.center.x;
        return buttonCenterX - activeIndicator.rect.center.x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnifiedText))]
public class CountingText : MonoBehaviourWithInit
{
    [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("duration")]
    private float defaultDuration;
    [SerializeField]
    private double minDeltaValue;

    [Space]
    [SerializeField]
    private bool useUnscaledDeltaTime = true;

    [Space]
    [SerializeField]
    private UnityEvent onStartCounting;
    [SerializeField]
    private UnityEvent onStopCounting;

    private UnifiedText unifiedText;

    private bool isCounting;
    private double startValue;
    private double endValue;
    private float currentTime;
    private float duration;
    private System.Action callback;

    public double CurrentValue { get; private set; }

    protected override bool Init()
    {
        ///
        unifiedText = GetComponent<UnifiedText>();

        ///
        return base.Init();
    }

    protected void OnEnable()
    {
        DisplayCurrentValue();
    }

    public void StartCounting(double startValue, double endValue)
    {
        ///
        TryInit();

        ///
        StartCounting(startValue, endValue, defaultDuration, null);
    }

    public void StartCounting(double startValue, double endValue, float duration, System.Action callback)
    {
        ///
        TryInit();

        ///
        if (System.Math.Abs(endValue - startValue) < minDeltaValue)
        {
            ///
            CurrentValue = endValue;
            DisplayCurrentValue();

            ///
            callback?.Invoke();

            ///
            return;
        }

        ///
        CurrentValue = startValue;
        this.startValue = startValue;
        this.endValue = endValue;
        this.duration = duration;
        this.callback = callback;
        isCounting = true;
        currentTime = 0;

        ///
        onStartCounting?.Invoke();
    }

    protected void Update()
    {
        ///
        if (!isCounting)
        {
            return;
        }

        ///
        var deltaTime = useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
        currentTime = Mathf.MoveTowards(currentTime, duration, deltaTime);

        ///
        if (Mathf.Approximately(currentTime, duration))
        {
            ///
            currentTime = duration;
            isCounting = false;
            CurrentValue = endValue;

            ///
            callback?.Invoke();

            ///
            onStopCounting?.Invoke();
        }
        else
        {
            CurrentValue = (currentTime / duration) * (endValue - startValue) + startValue;
        }

        ///
        DisplayCurrentValue();
    }

    private void DisplayCurrentValue()
    {
        unifiedText.Text = CurrentValue.ToLargeNumberString();
    }
}

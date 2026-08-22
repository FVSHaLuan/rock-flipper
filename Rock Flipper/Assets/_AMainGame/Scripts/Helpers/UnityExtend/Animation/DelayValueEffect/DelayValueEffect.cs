using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayValueEffect : MonoBehaviour
{
    public event System.Action OnUpdatedCurrentValue;

    [SerializeField]
    private float targetValue;
    [SerializeField]
    private float currentValue;

    [Space]
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private bool useUnscaledTime;

    [Space]
    [SerializeField]
    private float minChangingTime = 0.2f;

    [Space]
    [SerializeField]
    private UnityEvent onStartedChanging;
    [SerializeField]
    private UnityEvent onStoppedChanging;

    private bool isChangingCurrentValue;
    private bool isWaitingForMinChangingTime;
    private float changingTime = 0;

    public float TargetValue
    {
        get => targetValue;

        set => targetValue = value;
    }

    public float CurrentValue
    {
        get => currentValue;

        set => currentValue = value;
    }
    public float Speed { get => speed; set => speed = value; }

    protected void OnDisable()
    {
        CurrentValue = TargetValue;
        OnUpdatedCurrentValue?.Invoke();
    }

    protected void Update()
    {
        ///
        var savedChangingState = isChangingCurrentValue;

        ///
        var deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        ///
        if (isChangingCurrentValue)
        {
            var effectiveSpeed = Speed + changingTime * acceleration;
            currentValue = Mathf.MoveTowards(currentValue, targetValue, effectiveSpeed * deltaTime);

            ///
            OnUpdatedCurrentValue?.Invoke();
        }

        ///
        if (isChangingCurrentValue || isWaitingForMinChangingTime)
        {
            ///
            changingTime += deltaTime;
        }

        ///
        isChangingCurrentValue = !Mathf.Approximately(currentValue, targetValue);

        ///
        if (savedChangingState != isChangingCurrentValue)
        {
            ///
            if (isChangingCurrentValue)
            {
                ///
                changingTime = 0;
                isWaitingForMinChangingTime = true;

                ///
                onStartedChanging?.Invoke();
            }
            else if (changingTime >= minChangingTime)
            {
                StopChanging();
            }
        }
        else if (!isChangingCurrentValue && isWaitingForMinChangingTime && changingTime >= minChangingTime)
        {
            StopChanging();
        }
    }

    private void StopChanging()
    {
        ///
        changingTime = 0;
        isWaitingForMinChangingTime = false;

        ///
        onStoppedChanging?.Invoke();
    }
}

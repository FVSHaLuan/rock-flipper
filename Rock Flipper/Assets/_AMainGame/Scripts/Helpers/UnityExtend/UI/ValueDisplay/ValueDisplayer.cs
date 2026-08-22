using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ValueDisplayer<T> : MonoBehaviour where T : System.IComparable<T>
{
    [SerializeField]
    private bool doNotUpdate;

    [Space]
    [SerializeField]
    private UnityEvent onNewValue;

    private T lastValue;

    protected T LastValue => lastValue;

    protected abstract T GetCurrentValue();
    protected abstract void Display(bool isFirst, T previousValue, T currentValue);

    protected bool ignoreNextNewValueEvent;

    protected virtual bool IsDifferentValue(T oldValue, T newValue)
    {
        return oldValue.CompareTo(newValue) != 0;
    }

    protected virtual void OnEnable()
    {
        lastValue = GetCurrentValue();
        Display(true, default(T), lastValue);
    }

    protected virtual void Update()
    {
        if (doNotUpdate) return;
        DoUpdate();
    }

    protected void DoUpdate()
    {
        ///
        var currentValue = GetCurrentValue();

        ///
        if (IsDifferentValue(currentValue, lastValue))
        {
            ///
            Display(false, lastValue, currentValue);

            ///
            lastValue = currentValue;

            ///
            if (!ignoreNextNewValueEvent)
            {
                onNewValue?.Invoke();
            }
            else
            {
                ignoreNextNewValueEvent = false;
            }
        }
    }

    protected void Refresh()
    {
        Display(true, lastValue, lastValue);
    }

    protected void ForceDisplay()
    {
        var lastValue = GetCurrentValue();
        Display(false, lastValue, lastValue);
    }
}

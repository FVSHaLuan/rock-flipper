using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ValueDisplayerUnified<T> : ValueDisplayer<T> where T : System.IComparable<T>
{
    [SerializeField]
    protected UnifiedText unifiedText;

    protected void SetTextActive(bool isActive)
    {
        unifiedText.gameObject.SetActive(isActive);

    }

    protected virtual string GetString(T value)
    {
        return value.ToString();
    }

    protected override void Display(bool isFirst, T previousValue, T currentValue)
    {
        unifiedText.Text = GetString(currentValue);
    }

    public virtual void Reset()
    {
        unifiedText = GetComponent<UnifiedText>();
    }
}
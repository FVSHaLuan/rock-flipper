using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class TemporaryValue
{
    [SerializeField]
    private float maxValue = 10;
    [SerializeField]
    private float restoringSpeed = 1;
    [SerializeField]
    private float maxIncreasingTime = 0.2f;

    private StackedValue stackedValue = new StackedValue();

    private float value;
    private float targetValue;
    private bool isIncreasing;

    public float CurrentProgress => Mathf.Clamp01((value + stackedValue.Value) / maxValue);

    public int AddStackValue(float value)
    {
        return stackedValue.AddStackValue(value);
    }

    public bool RemoveStackValue(int stackId)
    {
        return stackedValue.RemoveStackValue(stackId);
    }

    public void AddValue(float amount, bool immediately = false)
    {
        ///
        targetValue = Mathf.MoveTowards(targetValue, maxValue, amount);
        if (immediately)
        {
            value = Mathf.MoveTowards(value, targetValue, amount);
        }

        ///
        isIncreasing = true;
    }

    public void SetValue(float amount)
    {
        ///
        value = targetValue = amount;

        ///
        isIncreasing = false;
    }

    public void SetToMax()
    {
        ///
        value = targetValue = maxValue;

        ///
        isIncreasing = false;
    }

    public void Clear()
    {
        ///
        value = targetValue = 0;
        stackedValue.Clear();

        ///
        isIncreasing = false;
    }

    public void ClearStackedValueOnly()
    {
        stackedValue.Clear();
    }

    public void SetProgress(float progress)
    {
        ///
        progress = Mathf.Clamp01(progress);

        ///
        value = targetValue = progress * maxValue;

        ///
        isIncreasing = false;
    }

    public void Update(float deltaTime)
    {
        if (!isIncreasing)
        {
            targetValue = value = Mathf.MoveTowards(value, 0, deltaTime * restoringSpeed);
        }
        else
        {
            ///
            var increasingSpeed = maxValue / maxIncreasingTime;

            ///
            value = Mathf.MoveTowards(value, targetValue, deltaTime * increasingSpeed);
            if (Mathf.Approximately(value, targetValue))
            {
                isIncreasing = false;
            }
        }
    }

    public static void ClearStackedValue(TemporaryValue temporaryValue)
    {
        temporaryValue.ClearStackedValueOnly();
    }
}

public delegate void TemporaryValueManipulator(TemporaryValue temporaryValue);

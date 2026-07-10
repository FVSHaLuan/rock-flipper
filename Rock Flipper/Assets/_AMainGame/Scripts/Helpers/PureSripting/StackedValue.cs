using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedValue
{
    public const int InvalidStackId = int.MinValue;

    public Dictionary<int, float> stackedValues = new Dictionary<int, float>();

    private float value;
    private int currentStackId = int.MinValue;

    public float Value => value;

    public int AddStackValue(float stackValue)
    {
        ///
        currentStackId++;

        ///
        stackedValues.Add(currentStackId, stackValue);
        value += stackValue;

        ///
        return currentStackId;
    }

    public bool RemoveStackValue(int stackId)
    {
        ///
        float stackValue;

        ///
        if (stackedValues.TryGetValue(stackId, out stackValue))
        {
            ///
            stackedValues.Remove(stackId);

            ///
            if (stackedValues.Count > 0)
            {
                value -= stackValue;
            }
            else
            {
                value = 0;
            }

            ///
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Clear()
    {
        stackedValues.Clear();
        value = 0;
    }
}

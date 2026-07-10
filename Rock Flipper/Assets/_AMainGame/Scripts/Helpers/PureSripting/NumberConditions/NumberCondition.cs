using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NumberCondition<T> where T : IComparable
{
    public T conditionerValue;
    public bool equal;
    public bool less;
    public bool more;

    public bool Check(T value)
    {
        ///
        var comparison = value.CompareTo(conditionerValue);

        ///
        if (equal && comparison == 0)
        {
            return true;
        }
        else if (less && comparison < 0)
        {
            return true;
        }
        else if (more && comparison > 0)
        {
            return true;
        }

        ///
        return false;
    }

    public static NumberCondition<T> Equal(T conditionerValue)
    {
        return new NumberCondition<T>()
        {
            conditionerValue = conditionerValue,
            equal = true,
            less = false,
            more = false
        };
    }

    public static NumberCondition<T> EqualOrMore(T conditionerValue)
    {
        return new NumberCondition<T>()
        {
            conditionerValue = conditionerValue,
            equal = true,
            less = true,
            more = false
        };
    }

    public static NumberCondition<T> EqualOrLess(T conditionerValue)
    {
        return new NumberCondition<T>()
        {
            conditionerValue = conditionerValue,
            equal = true,
            less = false,
            more = true
        };
    }
}
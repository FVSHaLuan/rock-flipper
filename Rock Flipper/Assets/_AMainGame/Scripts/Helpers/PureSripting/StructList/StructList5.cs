using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StructList5<T>
{
    public int Capacity => 5;

    [field: SerializeField]
    private T Value_0 { get; set; }
    [field: SerializeField]
    private T Value_1 { get; set; }
    [field: SerializeField]
    private T Value_2 { get; set; }
    [field: SerializeField]
    private T Value_3 { get; set; }
    [field: SerializeField]
    private T Value_4 { get; set; }

    [field: SerializeField]
    public int Count { get; private set; }

    public T this[int i]
    {
        get
        {
            return GetValue(i);
        }

        set
        {
            SetValue(i, value);
        }
    }

    private T GetValue(int index)
    {
        ///
        if (index < 0 || index >= Count)
        {
            throw new System.IndexOutOfRangeException();
        }

        ///
        switch (index)
        {
            case 0:
                return Value_0;
            case 1:
                return Value_1;
            case 2:
                return Value_2;
            case 3:
                return Value_3;
            case 4:
                return Value_4;
            default:
                throw new System.IndexOutOfRangeException();
        }
    }

    private void SetValue(int index, T value)
    {
        ///
        if (index < 0 || index >= Count)
        {
            throw new System.IndexOutOfRangeException();
        }

        ///
        switch (index)
        {
            case 0:
                Value_0 = value;
                break;
            case 1:
                Value_1 = value;
                break;
            case 2:
                Value_2 = value;
                break;
            case 3:
                Value_3 = value;
                break;
            case 4:
                Value_4 = value;
                break;
            default:
                throw new System.IndexOutOfRangeException();
        }
    }

    public void Add(T value)
    {
        ///
        if (Count >= Capacity)
        {
            throw new System.OverflowException();
        }

        ///
        Count++;

        ///
        SetValue(Count - 1, value);
    }

    public void Clear()
    {
        Count = 0;
    }

    public bool Contains(T value)
    {
        ///
        for (int i = 0; i < Count; i++)
        {
            if (this[i].Equals(value))
            {
                return true;
            }
        }

        ///
        return false;
    }
}

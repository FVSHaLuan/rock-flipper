using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StructList10<T>
{
    public int Capacity => 10;

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
    private T Value_5 { get; set; }
    [field: SerializeField]
    private T Value_6 { get; set; }
    [field: SerializeField]
    private T Value_7 { get; set; }
    [field: SerializeField]
    private T Value_8 { get; set; }
    [field: SerializeField]
    private T Value_9 { get; set; }

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
            case 5:
                return Value_5;
            case 6:
                return Value_6;
            case 7:
                return Value_7;
            case 8:
                return Value_8;
            case 9:
                return Value_9;
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
            case 5:
                Value_5 = value;
                break;
            case 6:
                Value_6 = value;
                break;
            case 7:
                Value_7 = value;
                break;
            case 8:
                Value_8 = value;
                break;
            case 9:
                Value_9 = value;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListPool<T>
{
    private List<PooledList<T>> lists;

    public PooledList<T> TakeInstance(int capacity)
    {
        ///
        PooledList<T> list = null;

        ///
        if (lists == null || lists.Count == 0)
        {
            ///
            list = new PooledList<T>(capacity);

            ///
            list.ListPool = this;
        }
        else
        {
            list = lists[lists.Count - 1];
            lists.RemoveAt(lists.Count - 1);
        }

        ///
        list.IsInPool = false;

        ///
        return list;
    }

    public void PushInstance(PooledList<T> list)
    {
        ///
        list.ListPool = this;
        list.IsInPool = true;

        ///
        if (lists == null)
        {
            lists = new List<PooledList<T>>();
        }

        ///
        lists.Add(list);
    }
}

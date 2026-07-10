using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledList<T> : List<T>, IDisposable
{
    public ListPool<T> ListPool { get; set; }
    public bool IsInPool { get; set; }

    public PooledList(int capacity) : base(capacity)
    {
    }

    public void Dispose()
    {
        TryGoToPool();
    }

    public bool TryGoToPool()
    {
        ///
        if (ListPool == null || IsInPool)
        {
            return false;
        }

        ///
        ListPool.PushInstance(this);

        ///
        return true;
    }
}

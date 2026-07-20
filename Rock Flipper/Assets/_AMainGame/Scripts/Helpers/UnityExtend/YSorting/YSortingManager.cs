using System.Collections.Generic;
using UnityEngine;

public class YSortingManager : MonoBehaviourWithInit
{
    private static YSortingManager instance;
    public static YSortingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<YSortingManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("YSortingManager");
                    instance = obj.AddComponent<YSortingManager>();
                }
            }

            ///
            return instance;
        }
    }

    private HashSet<YSortingListener> allListeners = new HashSet<YSortingListener>();
    private Dictionary<int, List<YSortingListener>> sortedListeners = new Dictionary<int, List<YSortingListener>>();

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        instance = this;
    }

    public void RegisterListener(YSortingListener listener)
    {
        allListeners.Add(listener);
    }

    public void UnregisterListener(YSortingListener listener)
    {
        allListeners.Remove(listener);
    }

    protected void LateUpdate()
    {
        ///
        foreach (var listeners in sortedListeners.Values)
        {
            listeners.Clear();
        }

        ///
        foreach (var listener in allListeners)
        {
            if (!sortedListeners.TryGetValue(listener.SortingLayer, out var listeners))
            {
                listeners = new List<YSortingListener>();
                sortedListeners[listener.SortingLayer] = listeners;
            }
            listeners.Add(listener);
        }

        ///
        foreach (var listeners in sortedListeners.Values)
        {
            listeners.Sort(YSortingListener.Compare);
            int sortingOrder = 32767;
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].SortingOrder = sortingOrder--;
            }
        }
    }
}

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

    private HashSet<YSortingListener> listeners = new HashSet<YSortingListener>();

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        instance = this;
    }

    public void RegisterListener(YSortingListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(YSortingListener listener)
    {
        listeners.Remove(listener);
    }
}

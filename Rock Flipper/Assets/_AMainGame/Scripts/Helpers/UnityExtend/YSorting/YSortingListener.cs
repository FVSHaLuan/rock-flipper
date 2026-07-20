using UnityEngine;

public abstract class YSortingListener : MonoBehaviour
{
    public abstract int SortingLayer { get; }
    public int SortingOrder { get; set; }
    public float Y { get; }

    protected void OnDisable()
    {
        YSortingManager.Instance.UnregisterListener(this);
    }

    protected void OnEnable()
    {
        YSortingManager.Instance.RegisterListener(this);
    }

    public static int Compare(YSortingListener a, YSortingListener b)
    {
        if (a.SortingLayer != b.SortingLayer)
        {
            return a.SortingLayer.CompareTo(b.SortingLayer);
        }

        ///
        return a.Y.CompareTo(b.Y);
    }
}

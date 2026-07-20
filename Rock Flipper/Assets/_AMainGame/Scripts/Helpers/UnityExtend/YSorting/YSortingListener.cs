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
}

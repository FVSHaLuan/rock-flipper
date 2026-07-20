using UnityEngine;

public abstract class YSortingListener : MonoBehaviour
{
    public abstract int SortingLayer { get; }
    public int SortingOrder { get; set; }
}

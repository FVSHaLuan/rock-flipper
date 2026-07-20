using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class YSortingGroupListener : YSortingListener
{
    private SortingGroup sortingGroup;

    private SortingGroup SortingGroup
    {
        get
        {
            if (sortingGroup == null)
            {
                sortingGroup = GetComponent<SortingGroup>();
            }
            return sortingGroup;
        }
    }

    public override int SortingLayer => SortingGroup.sortingLayerID;

    public override int SortingOrder { get => SortingGroup.sortingOrder; set => SortingGroup.sortingOrder = value; }

    public override float Y => transform.position.y;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class SortingGroupSetter : MonoBehaviour
{
    [SerializeField]
    private SortingGroup targetSortingGroup;

    public void SetSortingLayer()
    {
        targetSortingGroup.sortingLayerID = GetComponent<SortingGroup>().sortingLayerID;
    }
}

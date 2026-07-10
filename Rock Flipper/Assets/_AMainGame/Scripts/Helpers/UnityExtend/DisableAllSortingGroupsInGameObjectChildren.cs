using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class DisableAllSortingGroupsInGameObjectChildren
{
    private static List<SortingGroup> sortingGroups = new List<SortingGroup>();

    public static void DisableAllSortingGroupsInChildren(this GameObject gameObject, bool includeInactiveChildren)
    {
        ///
        gameObject.transform.GetComponentsInChildren(includeInactiveChildren, sortingGroups);

        ///
        for (int i = 0; i < sortingGroups.Count; i++)
        {
            sortingGroups[i].enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsLayerSetter : MonoBehaviour
{
    [SerializeField, UnityLayer]
    private int layer;
    [SerializeField]
    private List<GameObject> targetObjects;

    [ContextMenu("Set")]
    public void Set()
    {
        foreach (var item in targetObjects)
        {
            item.layer = layer;
        }
    }
}

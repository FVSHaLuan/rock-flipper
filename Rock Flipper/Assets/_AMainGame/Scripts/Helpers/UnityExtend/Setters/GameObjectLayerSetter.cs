using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectLayerSetter : MonoBehaviour
{
    [SerializeField, UnityLayer]
    private int layer;
    [SerializeField]
    private GameObject targetObject;

    [ContextMenu("Set")]
    public void Set()
    {
        targetObject.layer = layer;
    }
}

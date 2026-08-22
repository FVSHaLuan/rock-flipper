using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAlphaHitTestThresholdSetter : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    private float alphaHitTestMinimumThreshold;

    protected void OnEnable()
    {
        Set();
    }

    [ContextMenu("Set")]
    public void Set()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = alphaHitTestMinimumThreshold;
    }
}

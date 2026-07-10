using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ScrollRectSelectionSnapItem : MonoBehaviour
{
    [SerializeField]
    private RectTransform refRectTransform;

    public RectTransform RefRectTransform
    {
        get
        {
            if (refRectTransform == null)
            {
                return GetComponent<RectTransform>();
            }
            else
            {
                return refRectTransform;
            }
        }
    }
}

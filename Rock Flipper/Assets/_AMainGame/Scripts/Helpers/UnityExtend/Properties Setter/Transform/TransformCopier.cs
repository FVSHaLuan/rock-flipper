using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCopier : MonoBehaviour
{
    [SerializeField]
    private Transform source;
    [SerializeField]
    private Transform destination;

    [Space]
    [SerializeField]
    private bool copyPosition = true;
    [SerializeField]
    private bool copyRotation = true;
    [SerializeField]
    private bool copyScale = true;

    [Space]
    [SerializeField]
    private bool useLocalPosition;
    [SerializeField]
    private bool useLocalRotation;

    public void Copy()
    {
        // Pos
        if (copyPosition)
        {
            if (useLocalPosition)
            {
                destination.localPosition = source.localPosition;
            }
            else
            {
                destination.position = source.position;
            }
        }

        // Rotation
        if (copyRotation)
        {
            if (useLocalRotation)
            {
                destination.localRotation = source.localRotation;
            }
            else
            {
                destination.rotation = source.rotation;
            }
        }

        // Scale
        if (copyScale)
        {
            destination.localScale = source.localScale;
        }
    }

#if UNITY_EDITOR
    protected void Reset()
    {
        destination = transform;
    }
#endif
}

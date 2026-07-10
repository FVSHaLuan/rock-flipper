using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnyKeyEvents : UIScreenChildInteraction
{
    [SerializeField]
    private UnityEvent onAnyKeyPressedThisFrame;

    protected void OnEnable()
    {      
        entry.anyKeyDetector.OnAnyKeyPressedThisFrame += AnyKeyDetector_OnAnyKeyPressedThisFrame;
    }

    protected void OnDisable()
    {
        entry.anyKeyDetector.OnAnyKeyPressedThisFrame -= AnyKeyDetector_OnAnyKeyPressedThisFrame;
    }

    private void AnyKeyDetector_OnAnyKeyPressedThisFrame()
    {
        if (Interactable)
        {
            onAnyKeyPressedThisFrame?.Invoke();
        }
    }
}

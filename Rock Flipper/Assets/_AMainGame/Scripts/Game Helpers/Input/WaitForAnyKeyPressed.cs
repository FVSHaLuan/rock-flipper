using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForAnyKeyPressed : CustomYieldInstruction
{
    private bool anyKeyPressed = false;

    public override bool keepWaiting => !anyKeyPressed;

    public WaitForAnyKeyPressed()
    {
        Entry.Instance.anyKeyDetector.OnAnyKeyPressedThisFrame += AnyKeyDetector_OnAnyKeyPressedThisFrame;
    }

    private void AnyKeyDetector_OnAnyKeyPressedThisFrame()
    {
        ///
        anyKeyPressed = true;

        ///
        Entry.Instance.anyKeyDetector.OnAnyKeyPressedThisFrame -= AnyKeyDetector_OnAnyKeyPressedThisFrame;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Entry
{
    private int lastPointerPosUpdatedFrame = -1;
    private Vector2 lastPointerWorldPosition;

    public Vector2 GetPointerPositionViaConversionCamera()
    {
        if (lastPointerPosUpdatedFrame != Time.frameCount)
        {
            var mousePos = Mouse.current.position.ReadValue();
            lastPointerWorldPosition = conversionCamera.ScreenToWorldPoint(mousePos);
        }

        ///
        return lastPointerWorldPosition;
    }
}

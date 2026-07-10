using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// set alpha only
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupColoredObject : UnifiedColoredObject
{
    private CanvasGroup canvasGroup;

    protected override void SetColor(Color color)
    {
        ///
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        ///
        canvasGroup.alpha = color.a;
    }
}

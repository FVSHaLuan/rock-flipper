using FH.Core.Gameplay.HelperComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkerColor : Blinker
{
    [Header("BlinkerColor")]
    [SerializeField]
    private UnifiedColoredObject unifiedColoredObject;
    [SerializeField]
    private Color visibleColor = Color.white;
    [SerializeField]
    private Color invisibleColor = Color.gray;

    private bool isVisible = true;

    protected override bool IsVisible
    {
        get => isVisible;
        set
        {
            ///
            isVisible = value;

            ///
            var color = isVisible ? visibleColor : invisibleColor;
            unifiedColoredObject.Color = color;
        }
    }
}

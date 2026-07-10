using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI.EnhancedScroller;

public class ButtonEnhancedScrollerGamepad : ButtonScrollerGamepad
{
    [Header("ButtonEnhancedScrollerGamepad")]
    [SerializeField]
    private EnhancedScroller enhancedScroller;

    public override void Update()
    {
        ///        
        rectTransformToScroll = enhancedScroller.Container;

        ///
        base.Update();
    }
}

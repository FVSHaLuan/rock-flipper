using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenChildInteraction : ExtendedMonoBehaviour
{
    private UIScreen uiScreen;

    protected bool Interactable
    {
        get
        {
            ///
            if (uiScreen != null && (!uiScreen.Interactable || !uiScreen.IsScreenActive))
            {
                return false;
            }

            ///
            return true;
        }
    }

    protected override bool Init()
    {
        ///
        uiScreen = GetComponentInParent<UIScreen>();

        ///
        return base.Init();
    }
}

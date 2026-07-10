using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleContextPrompt : ContextButtonPrompt
{
    [SerializeField]
    private List<UIInputActionBase> uIInputActionBases;

    public void OnEnable()
    {
        AddToTheManager();
    }

    public void OnDisable()
    {
        RemoveFromTheManager();
    }

    protected void Update()
    {
        UpdateInteractability();
    }

    private void UpdateInteractability()
    {
        if (uIInputActionBases == null || uIInputActionBases.Count <= 0)
        {
            Interactable = !UIInputActionBase.Disabled;
        }
        else
        {
            for (int i = 0; i < uIInputActionBases.Count; i++)
            {
                if (!uIInputActionBases[i].interactable)
                {
                    ///
                    Interactable = false;

                    ///
                    return;
                }
            }

            ///
            Interactable = true;
        }
    }
}

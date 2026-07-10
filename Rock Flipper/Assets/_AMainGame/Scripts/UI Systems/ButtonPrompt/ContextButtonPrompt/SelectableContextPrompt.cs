using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectableContextPrompt : ContextButtonPrompt
{
    private Selectable selectable;

    protected override void ExtendedAwake()
    {
        selectable = GetComponent<Selectable>();
    }

    public void OnEnable()
    {
        ///
        if (entry.uiSelectedEventManager.LastGameObject == gameObject)
        {
            AddToTheManager();
        }

        ///
        entry.uiSelectedEventManager.OnSelectionChanged += UiSelectedEventManager_OnSelectionChanged;
    }

    public void OnDisable()
    {
        ///
        if (IsInTheManager)
        {
            RemoveFromTheManager();
        }

        ///
        entry.uiSelectedEventManager.OnSelectionChanged -= UiSelectedEventManager_OnSelectionChanged;
    }

    private void UiSelectedEventManager_OnSelectionChanged()
    {
        ///
        if (entry.uiSelectedEventManager.LastGameObject == gameObject)
        {
            AddToTheManager();
        }
        else
        {
            if (IsInTheManager)
            {
                RemoveFromTheManager();
            }
        }
    }

    protected void Update()
    {
        Interactable = selectable.IsInteractable() && !entry.completeInputBlocker.IsBlocking;
    }
}

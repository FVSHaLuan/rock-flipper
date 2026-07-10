using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UISelectedEvent : ExtendedMonoBehaviour
{
    [SerializeField]
    private GameObject selectedView = null;
    [SerializeField]
    private GameObject deselectedView = null;

    [Space]
    [SerializeField]
    private bool checkOnEnable = true;
    [SerializeField]
    private bool playNavigationSound = true;

    [Space]
    [SerializeField]
    private UnityEvent onSelected;
    [SerializeField]
    private UnityEvent onDeselected;

    private int enabledFrameCount = -1;

    public void OnEnable()
    {
        ///
        enabledFrameCount = Time.frameCount;

        ///
        View();

        ///
        entry.uiSelectedEventManager.OnSelectionChanged += UISelectedEventManager_OnSelectionChanged;

        ///
        if (checkOnEnable)
        {
            UISelectedEventManager_OnSelectionChanged();
        }
    }

    private void UISelectedEventManager_OnSelectionChanged()
    {
        ///
        var selected = entry.uiSelectedEventManager.LastGameObject == gameObject;

        // Play sound
        if (selected && playNavigationSound && Time.frameCount > enabledFrameCount)
        {
            entry.uiSoundManager.PlayNavigationSound();
        }

        ///
        View(selected);
    }

    public void OnDisable()
    {
        ///
        entry.uiSelectedEventManager.OnSelectionChanged -= UISelectedEventManager_OnSelectionChanged;
    }

    private void View()
    {
        var selected = entry.uiSelectedEventManager.LastGameObject == gameObject;
        View(selected);
    }

    private void View(bool selected)
    {
        if (selected)
        {
            ViewSelected();
        }
        else
        {
            ViewDeselected();
        }
    }

    private void ViewSelected()
    {
        ///
        if (selectedView != null)
        {
            selectedView?.SetActive(true);
        }
        if (deselectedView != null)
        {
            deselectedView?.SetActive(false);
        }

        ///
        onSelected?.Invoke();
    }

    private void ViewDeselected()
    {
        ///
        if (selectedView)
        {
            selectedView?.SetActive(false);
        }
        if (deselectedView)
        {
            deselectedView?.SetActive(true);
        }

        ///
        onDeselected?.Invoke();
    }

}

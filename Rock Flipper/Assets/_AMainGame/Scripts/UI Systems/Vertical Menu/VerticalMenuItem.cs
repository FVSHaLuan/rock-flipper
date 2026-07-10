using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class VerticalMenuItem : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedObject;
    [SerializeField]
    private GameObject activeMenuObject;
    [SerializeField]
    private GameObject sectionContent;

    private bool isActiveMenu;

    public GameObject SectionContent => sectionContent;

    public bool IsActiveMenu
    {
        get
        {
            return isActiveMenu;
        }

        set
        {
            ///
            isActiveMenu = value;

            ///
            sectionContent.SetActive(value);

            ///
            UpdateView();
        }
    }

    public void OnEnable()
    {
        ///
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        ///
        UpdateView();

        ///
        Entry.Instance.uiSelectedEventManager.OnSelectionChanged += UiSelectedEventManager_OnSelectionChanged;
    }

    public void OnDisable()
    {
        ///
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        ///
        Entry.Instance.uiSelectedEventManager.OnSelectionChanged -= UiSelectedEventManager_OnSelectionChanged;
    }

    private void UiSelectedEventManager_OnSelectionChanged()
    {
        UpdateView();
    }

    private void UpdateView()
    {
        bool isSelected = Entry.Instance.uiSelectedEventManager.LastGameObject == gameObject;

        ///
        selectedObject.SetActive(isSelected);
        activeMenuObject.SetActive(!isSelected && isActiveMenu);
    }
}

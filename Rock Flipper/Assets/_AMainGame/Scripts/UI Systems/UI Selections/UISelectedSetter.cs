using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectedSetter : MonoBehaviour
{
    [SerializeField]
    private GameObject selectionGameObject;
    [SerializeField]
    private bool setOnEnable = true;

    public void OnEnable()
    {
        if (setOnEnable)
        {
            Set();
        }
    }

    public void Set()
    {
        Entry.Instance.uiSelectedEventManager.SetCurrentSelectedGameObject(selectionGameObject);
    }

    public void SetIfCurrentOneNullOrNotActive()
    {
        ///
        var selectedObject = EventSystem.current.currentSelectedGameObject;

        ///
        if (selectedObject == null
            || !selectedObject.activeInHierarchy)
        {
            Set();
        }
    }
}

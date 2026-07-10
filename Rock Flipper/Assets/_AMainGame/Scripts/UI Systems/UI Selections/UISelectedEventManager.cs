using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectedEventManager : MonoBehaviourWithInit
{
    public event System.Action OnSelectionChanged;

    private GameObject lastGameObject;
    private GameObject lastNonNullGameObject;
    private Selectable lastSelectable;
    private SelectableInfo lastSelectableInfo;

    public GameObject LastGameObject
    {
        get
        {
            ///
            TryInit();

            ///
            return lastGameObject;
        }

        private set
        {
            lastGameObject = value;

            ///
            lastSelectable = lastGameObject == null ? null : lastGameObject.GetComponent<Selectable>();
            lastSelectableInfo = lastGameObject == null ? null : lastGameObject.GetComponent<SelectableInfo>();

            ///
            if (lastGameObject != null)
            {
                lastNonNullGameObject = lastGameObject;
            }
        }
    }
    public GameObject LastNonNullGameObject
    {
        get
        {
            ///
            TryInit();

            ///
            return lastNonNullGameObject;
        }
    }
    public Selectable LastSelectable
    {
        get
        {
            TryInit();
            return lastSelectable;
        }
    }
    public SelectableInfo LastSelectableInfo
    {
        get
        {
            TryInit();
            return lastSelectableInfo;
        }
    }

    protected override bool Init()
    {
        ///
        LastGameObject = EventSystem.current.currentSelectedGameObject;

        ///
        return base.Init();
    }

    public bool SetCurrentSelectedGameObject(GameObject go)
    {
        ///
        if (go == EventSystem.current.currentSelectedGameObject)
        {
            return false;
        }

        ///
        LastGameObject = go;

        ///
        EventSystem.current.SetSelectedGameObject(go);

        ///
        OnSelectionChanged?.Invoke();

        ///
        return true;
    }

    public void Update()
    {
        ///
        var currentSelectedObject = EventSystem.current.currentSelectedGameObject;

        ///
        if (currentSelectedObject != LastGameObject)
        {
            ///
            if (currentSelectedObject != null)
            {
                ///
                var selectableInfo = currentSelectedObject.GetComponent<SelectableInfo>();

                ///
                if (selectableInfo != null && selectableInfo.NeverSelectThis)
                {
                    EventSystem.current.SetSelectedGameObject(LastGameObject);
                    return;
                }
            }

            ///
            LastGameObject = currentSelectedObject;

            ///
            OnSelectionChanged?.Invoke();
        }
    }
}

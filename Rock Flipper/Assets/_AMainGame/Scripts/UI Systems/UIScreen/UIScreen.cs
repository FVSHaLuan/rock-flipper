using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : ExtendedMonoBehaviour
{
    public event Action OnBecomeActive;
    public event Action OnBecomeInactive;
    public event Action OnInteractabilityChanged;

    [SerializeField]
    private List<Selectable> firstSelectables;
    [SerializeField]
    private List<Selectable> predefinedFirstSelectables;

    [Space]
    [SerializeField]
    private bool interactable = true;
    [SerializeField]
    private bool hidePreviousPopupContent = true;
    [SerializeField]
    private bool setFirstSelectable = true;
    [SerializeField]
    private bool ignoreLastSelectable;
    [SerializeField]
    private bool alwaysPopFromStackWhenDisable = true;
    [SerializeField]
    private bool setAsLastSiblingWhenBecomeActive = true;

    [Space]
    [SerializeField]
    private GameObject contentWrapper;

    [Space]
    [SerializeField]
    private UnityEvent onBecomeActive;
    [SerializeField]
    private UnityEvent onBecomeInactive;
    [SerializeField]
    private UnityEvent onHideContentRequest;
    [SerializeField]
    private UnityEvent onNotSetAnySelectable;

    private CanvasGroup canvasGroup;
    private Selectable lastSelectable;
    private string predefinedFirstSelectableFlag;

    public string PredefinedLastSelectableFlag
    {
        set
        {
            predefinedFirstSelectableFlag = value;
        }
    }
    public bool Interactable
    {
        get => interactable && IsScreenActive;
        set
        {
            ///
            if (interactable == value)
            {
                return;
            }

            ///
            interactable = value;

            ///
            OnInteractabilityChanged?.Invoke();
        }
    }
    public bool HidePreviousPopupContent => hidePreviousPopupContent;

    public bool IsScreenActive { get; private set; } = false;

    public bool IsBecomingActive { get; private set; }
    public bool IsBecomingInactive { get; private set; }

    protected override void ExtendedAwake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowPopup()
    {
        ///
        if (gameObject.activeSelf && !IsScreenActive)
        {
            PutToStack();
            enabled = true;
        }
        else
        {
            enabled = true;
            gameObject.SetActive(true);
        }
    }

    protected virtual void OnEnable()
    {
        if (!IsScreenActive)
        {
            PutToStack();
        }
    }

    protected virtual void OnDisable()
    {
        if (IsScreenActive || alwaysPopFromStackWhenDisable)
        {
            PopFromStack();
        }
    }

    protected void PutToStack()
    {
        ///
        entry.uiScreenManager.PutToStack(this);
    }

    public void PopFromStack()
    {
        ///
        entry.uiScreenManager.PopFromStack(this);
    }

    private void UiSelectedEventManager_OnSelectionChanged()
    {
        ///
        if (!IsScreenActive)
        {
            return;
        }

        ///
        var currentSelectedGameObject = entry.uiSelectedEventManager.LastGameObject;
        if (currentSelectedGameObject == null)
        {
            return;
        }
        var currentSelectable = currentSelectedGameObject.GetComponent<Selectable>();

        ///
        if (currentSelectedGameObject.GetComponentInParent<UIScreen>() == this)
        {
            lastSelectable = currentSelectable;
        }
    }

    public virtual void HandleBecomeActive()
    {
        ///
        if (IsBecomingInactive)
        {
            Debug.LogErrorFormat(gameObject, "Popup is becoming inactive! New active popup: {0}", entry.uiScreenManager.WaitingToBeActive?.gameObject);
        }

        ///
        IsBecomingActive = true;

        ///
        canvasGroup.interactable = true;
        IsScreenActive = true;

        ///
        if (contentWrapper != null)
        {
            contentWrapper.gameObject.SetActive(true);
        }

        ///
        bool selectedFirstSelectable = false;
        if (setFirstSelectable)
        {
            selectedFirstSelectable = SetFirstSelectable() != null;
        }
        if (!selectedFirstSelectable)
        {
            onNotSetAnySelectable?.Invoke();
        }

        ///
        entry.uiSelectedEventManager.OnSelectionChanged += UiSelectedEventManager_OnSelectionChanged;

        ///
        if (setAsLastSiblingWhenBecomeActive)
        {
            transform.SetAsLastSibling();
        }

        ///
        onBecomeActive?.Invoke();
        OnBecomeActive?.Invoke();

        ///
        IsBecomingActive = false;
    }

    public virtual void HandleBecomeInactive(bool requestToHideContent)
    {
        ///
        if (IsBecomingActive)
        {
            Debug.LogErrorFormat(gameObject, "Popup is becoming active! New active popup: {0}", entry.uiScreenManager.WaitingToBeActive?.gameObject);
        }

        ///
        IsBecomingInactive = true;

        ///
        canvasGroup.interactable = false;
        IsScreenActive = false;

        ///
        onBecomeInactive?.Invoke();
        OnBecomeInactive?.Invoke();

        ///
        if (requestToHideContent)
        {
            ///
            if (contentWrapper != null)
            {
                contentWrapper.gameObject.SetActive(false);
            }

            ///
            onHideContentRequest?.Invoke();

            ///
            entry.uiSelectedEventManager.OnSelectionChanged -= UiSelectedEventManager_OnSelectionChanged;
        }

        ///
        IsBecomingInactive = false;
    }

    private Selectable SetFirstSelectable()
    {
        // Predefined
        var predefinedFistSelectable = FindActivePredefinedFirstSelectable(predefinedFirstSelectableFlag);
        predefinedFirstSelectableFlag = null;
        if (predefinedFistSelectable != null)
        {
            entry.uiSelectedEventManager.SetCurrentSelectedGameObject(predefinedFistSelectable.gameObject);
            return predefinedFistSelectable;
        }

        ///
        var currentSelectedGameObject = entry.uiSelectedEventManager.LastGameObject;
        if (currentSelectedGameObject != null
            && currentSelectedGameObject.transform.IsChildOf(transform))
        {
            var currentSelectedSelectable = currentSelectedGameObject.GetComponent<Selectable>();
            if (currentSelectedSelectable != null)
            {
                lastSelectable = currentSelectedSelectable;
            }
        }

        ///
        if (lastSelectable != null
            && !ignoreLastSelectable
            && lastSelectable.gameObject.activeInHierarchy)
        {
            ///
            entry.uiSelectedEventManager.SetCurrentSelectedGameObject(lastSelectable.gameObject);

            ///
            return lastSelectable;
        }

        ///
        if (firstSelectables != null)
        {
            ///
            lastSelectable = null;

            ///
            for (int i = 0; i < firstSelectables.Count; i++)
            {
                ///
                var selectable = firstSelectables[i];

                ///
                if (selectable != null
                    && selectable.gameObject.activeInHierarchy)
                {
                    ///
                    lastSelectable = selectable;

                    ///
                    break;
                }
            }

            ///
            if (lastSelectable != null)
            {
                ///
                entry.uiSelectedEventManager.SetCurrentSelectedGameObject(lastSelectable.gameObject);

                ///
                return lastSelectable;
            }
        }

        ///
        entry.uiSelectedEventManager.SetCurrentSelectedGameObject(null);

        ///
        return null;
    }

    private Selectable FindActivePredefinedFirstSelectable(string selectableName)
    {
        ///
        if (string.IsNullOrWhiteSpace(selectableName))
        {
            return null;
        }

        ///
        if (predefinedFirstSelectables == null)
        {
            return null;
        }

        ///
        foreach (var item in predefinedFirstSelectables)
        {
            if (item.interactable
                && item.gameObject.activeInHierarchy
                && item.name == selectableName)
            {
                return item;
            }
        }

        ///
        return null;
    }
}

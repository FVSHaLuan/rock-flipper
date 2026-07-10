using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class ListItemSelectableRect : MonoBehaviour
{
    [SerializeField]
    private UnityEvent viewSelectedDelegate;
    [SerializeField]
    private UnityEvent viewUnselectedDelegate;

    private ListItemSelectionManager listItemSelectionManager;

    public bool IsSelected => listItemSelectionManager != null ? listItemSelectionManager.SelectedItem == this : false;

    public int RegistrationId { get; private set; }

    protected void Awake()
    {
        listItemSelectionManager = GetComponentInParent<ListItemSelectionManager>();
    }

    protected void OnEnable()
    {
        ///
        RegistrationId = listItemSelectionManager != null ? listItemSelectionManager.RegisterItem(this) : -1;

        ///
        UpdateView();

        ///
        listItemSelectionManager.OnSelectedItemChanged += ListItemSelectionManager_OnSelectedItemChanged;
    }

    protected void OnDisable()
    {
        ///
        listItemSelectionManager?.UnregisterItem(this);

        ///
        listItemSelectionManager.OnSelectedItemChanged -= ListItemSelectionManager_OnSelectedItemChanged;
    }

    private void ListItemSelectionManager_OnSelectedItemChanged()
    {
        UpdateView();
    }

    private void UpdateView()
    {
        if (IsSelected)
        {
            viewSelectedDelegate?.Invoke();
        }
        else
        {
            viewUnselectedDelegate?.Invoke();
        }
    }
}

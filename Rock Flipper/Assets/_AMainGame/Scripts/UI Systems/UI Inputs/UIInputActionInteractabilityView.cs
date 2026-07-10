using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIInputActionBase))]
public class UIInputActionInteractabilityView : MonoBehaviour
{
    [SerializeField]
    private GameObject interactableView;
    [SerializeField]
    private GameObject notInteractableView;

    private UIInputActionBase inputActionBase;

    public void Awake()
    {
        inputActionBase = GetComponent<UIInputActionBase>();
    }

    public void OnEnable()
    {
        UpdateViews();
    }

    public void Update()
    {
        UpdateViews();
    }

    private void UpdateViews()
    {
        if (interactableView != null)
        {
            interactableView.SetActive(inputActionBase.interactable);
        }
        if (notInteractableView != null)
        {
            notInteractableView.SetActive(!inputActionBase.interactable);
        }
    }
}

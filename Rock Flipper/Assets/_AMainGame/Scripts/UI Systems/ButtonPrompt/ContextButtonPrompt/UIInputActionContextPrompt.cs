using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIInputActionBase))]
public class UIInputActionContextPrompt : ContextButtonPrompt
{
    [Header("UIInputActionContextPrompt")]
    private bool removeWhenNotInteractable;
    [SerializeField]
    private UIInputActionBase additionalUIInputActionBase;

    private UIInputActionBase uiInputActionBase;

    public override UIInputActionBase UIInputActionBase
    {
        get
        {
            ///
            TryInit();

            ///
            return uiInputActionBase;
        }

        protected set => uiInputActionBase = value;
    }

    protected override bool Init()
    {
        ///
        uiInputActionBase = GetComponent<UIInputActionBase>();

        ///
        InputAction = uiInputActionBase.InputActionReference;
        UIInputActionBase = uiInputActionBase;

        ///
        return base.Init();
    }

    public void OnEnable()
    {
        ///
        Interactable = uiInputActionBase.IsEnabledAndInteractable;

        ///
        if (Interactable || !removeWhenNotInteractable)
        {
            AddToTheManager();
        }
    }

    public void OnDisable()
    {
        RemoveFromTheManager();
    }

    public void Update()
    {
        ///
        var currentInteractabilityState = uiInputActionBase.IsEnabledAndInteractable;

        ///
        if (currentInteractabilityState != Interactable)
        {
            ///
            Interactable = currentInteractabilityState;

            ///
            if (Interactable)
            {
                if (!IsInTheManager)
                {
                    AddToTheManager();
                }
            }
            else if (removeWhenNotInteractable)
            {
                RemoveFromTheManager();
            }
        }

        ///
        if (IsInTheManager)
        {
            AdditionalIconInteractable = additionalUIInputActionBase != null ? additionalUIInputActionBase.interactable : currentInteractabilityState;
        }
    }

#if UNITY_EDITOR
    public void Reset()
    {
        TryGetInputAction();
    }

    [ContextMenu("TryGetInputAction")]
    private void TryGetInputAction()
    {
        ///
        UnityEditor.Undo.RecordObject(this, "TryGetInputAction");

        ///
        InputAction = GetComponent<UIInputActionBase>().InputActionReference;
    }
#endif
}

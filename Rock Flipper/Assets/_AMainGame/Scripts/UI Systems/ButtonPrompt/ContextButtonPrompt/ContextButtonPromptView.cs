using BT.UI.ButtonPrompts;
using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextButtonPromptView : ExtendedMonoBehaviour
{
    [SerializeField]
    private ButtonPromptViewImage buttonPromptViewImage;
    [SerializeField]
    private UnifiedText unifiedText;
    [SerializeField]
    private Color holdTextColor = Color.green;
    [SerializeField]
    private ProgressBar holdProgressBar;
    [SerializeField]
    private GameObject disabledViewObject;

    [Space]
    [SerializeField]
    private GameObject additionalIconWrapper;
    [SerializeField]
    private ButtonPromptViewImage additionalButtonPromptViewImage;
    [SerializeField]
    private ProgressBar additionalHoldProgressBar;
    [SerializeField]
    private GameObject additionalDisabledViewObject;

    private ContextButtonPrompt contextButtonPrompt;
    private UIHoldAction uiHoldAction;

    public void SetPrompt(ContextButtonPrompt contextButtonPrompt)
    {
        ///
        ClearOldPromptReferences();

        ///
        holdProgressBar.SetValue(0);

        ///
        this.contextButtonPrompt = contextButtonPrompt;

        ///
        buttonPromptViewImage.InputAction = contextButtonPrompt.InputAction;

        ///
        if (contextButtonPrompt.UIInputActionBase != null && holdProgressBar != null)
        {
            uiHoldAction = contextButtonPrompt.UIInputActionBase as UIHoldAction;
            if (uiHoldAction != null)
            {
                uiHoldAction.OnUpdatedHoldProgress += UiHoldAction_OnUpdatedHoldProgress;
            }
        }
        else
        {
            uiHoldAction = null;
        }

        ///
        UpdateText();
        UpdateAdditionalIcon();
        UpdateDisabledViewObject();

        ///
        contextButtonPrompt.OnTextChanged += ContextButtonPrompt_OnTextChanged;
        contextButtonPrompt.OnInteractabilityChanged += ContextButtonPrompt_OnInteractabilityChanged;
        contextButtonPrompt.OnAdditionalIconChanged += ContextButtonPrompt_OnAdditionalIconChanged;
    }

    private void UpdateAdditionalIcon()
    {
        if (contextButtonPrompt.AdditionalInputAction != null)
        {
            ///
            additionalIconWrapper.gameObject.SetActive(true);

            ///
            additionalButtonPromptViewImage.InputAction = contextButtonPrompt.AdditionalInputAction;
        }
        else
        {
            additionalIconWrapper.gameObject.SetActive(false);
        }
    }

    private void UiHoldAction_OnUpdatedHoldProgress(float value)
    {
        holdProgressBar.SetValue(value);
    }

    private void ClearOldPromptReferences()
    {
        ///
        if (contextButtonPrompt != null)
        {
            contextButtonPrompt.OnTextChanged -= ContextButtonPrompt_OnTextChanged;
            contextButtonPrompt.OnInteractabilityChanged -= ContextButtonPrompt_OnInteractabilityChanged;
            contextButtonPrompt.OnAdditionalIconChanged -= ContextButtonPrompt_OnAdditionalIconChanged;
        }

        ///
        if (uiHoldAction != null)
        {
            uiHoldAction.OnUpdatedHoldProgress -= UiHoldAction_OnUpdatedHoldProgress;
        }
    }

    private void ContextButtonPrompt_OnAdditionalIconChanged()
    {
        UpdateAdditionalIcon();
        UpdateDisabledViewObject();
    }

    private void ContextButtonPrompt_OnInteractabilityChanged()
    {
        UpdateDisabledViewObject();
    }

    private void ContextButtonPrompt_OnTextChanged()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        var isHold = entry.buttonPromptManager.GetPromptSprites(contextButtonPrompt.InputAction).isHold;
        UpdateText(isHold);
    }

    private void UpdateDisabledViewObject()
    {
        ///
        var isInteractable = contextButtonPrompt.Interactable;

        ///
        if (disabledViewObject != null)
        {
            disabledViewObject.SetActive(!isInteractable);
        }

        ///
        if (contextButtonPrompt.AdditionalInputAction != null)
        {
            additionalDisabledViewObject.SetActive(!contextButtonPrompt.AdditionalIconInteractable);
        }
    }

    private void UpdateText(bool isHold)
    {
        ///
        if (!isHold)
        {
            unifiedText.Text = contextButtonPrompt.Text;
        }
        else
        {
            unifiedText.Text = string.Format("<color=#{0}>[HOLD]</color> {1}", ColorUtility.ToHtmlStringRGBA(holdTextColor), contextButtonPrompt.Text);
        }

        ///
        unifiedText.Color = contextButtonPrompt.TextColor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveContextButtonPromptsView : ExtendedMonoBehaviour
{
    [SerializeField]
    private ContextButtonPromptView firstViewItem;

    private List<ContextButtonPromptView> viewItems = new List<ContextButtonPromptView>();
    private List<ContextButtonPrompt> prompts = new List<ContextButtonPrompt>();

    private UIScreen uiScreen;

    protected override void ExtendedAwake()
    {
        ///
        uiScreen = GetComponentInParent<UIScreen>();
        firstViewItem.gameObject.SetActive(false);
        viewItems.Add(firstViewItem);
    }

    public void OnEnable()
    {
        ///
        entry.activeContextButtonPromptManager.OnActivePromptListChanged += ActiveContextButtonPromptManager_OnActivePromptListChanged;

        ///
        UpdateViews();
    }

    public void OnDisable()
    {
        entry.activeContextButtonPromptManager.OnActivePromptListChanged -= ActiveContextButtonPromptManager_OnActivePromptListChanged;
    }

    private void ActiveContextButtonPromptManager_OnActivePromptListChanged()
    {
        UpdateViews();
    }

    private void UpdateViews()
    {
        ///
        GetPrompts();

        ///
        while (viewItems.Count < prompts.Count)
        {
            var viewItem = Instantiate(firstViewItem, firstViewItem.transform.parent);
            viewItems.Add(viewItem);
        }

        ///
        for (int i = 0; i < viewItems.Count; i++)
        {
            ///
            var viewItem = viewItems[i];

            ///
            if (i < prompts.Count)
            {
                ///
                viewItem.gameObject.SetActive(true);

                ///
                var prompt = prompts[i];
                viewItem.SetPrompt(prompt);
            }
            else
            {
                ///
                viewItem.gameObject.SetActive(false);
            }
        }
    }

    private void GetPrompts()
    {
        ///
        prompts.Clear();

        ///
        var activePromptManager = entry.activeContextButtonPromptManager;

        ///
        for (int i = 0; i < activePromptManager.ActivePromptCount; i++)
        {
            ///
            var prompt = activePromptManager.GetActivePrompt(i);

            ///
            if (prompt.UIScreenParent == uiScreen)
            {
                prompts.Add(prompt);
            }
        }
    }
}

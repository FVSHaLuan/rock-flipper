using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveContextButtonPromptManager : MonoBehaviour
{
    public event Action OnActivePromptListChanged;

    private List<ContextButtonPrompt> activePrompts = new List<ContextButtonPrompt>();

    public int ActivePromptCount => activePrompts.Count;

    private bool dirtyFlag;

    public ContextButtonPrompt GetActivePrompt(int id)
    {
        return activePrompts[id];
    }

    public bool AddPrompt(ContextButtonPrompt contextButtonPrompt)
    {
        ///
        if (activePrompts.Contains(contextButtonPrompt))
        {
            return false;
        }

        ///
        int orderId = contextButtonPrompt.OrderId;

        ///
        if (activePrompts.Count > 0)
        {
            for (int i = 0; i < activePrompts.Count; i++)
            {
                if (activePrompts[i].OrderId <= orderId)
                {
                    ///
                    activePrompts.Insert(i, contextButtonPrompt);

                    ///
                    break;
                }
                else if (i == (activePrompts.Count - 1))
                {
                    activePrompts.Add(contextButtonPrompt);
                    break;
                }
            }
        }
        else
        {
            activePrompts.Add(contextButtonPrompt);
        }

        ///
        dirtyFlag = true;

        ///
        return true;
    }

    public bool RemovePrompt(ContextButtonPrompt contextButtonPrompt)
    {
        ///
        var rs = activePrompts.Remove(contextButtonPrompt);

        ///
        if (rs)
        {
            dirtyFlag = true;
        }

        ///
        return rs;
    }

    public void LateUpdate()
    {
        if (dirtyFlag)
        {
            ///
            dirtyFlag = false;

            ///
            OnActivePromptListChanged?.Invoke();
        }
    }
}

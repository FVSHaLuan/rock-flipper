using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMenuItem : ExtendedMonoBehaviour
{
    public event System.Action OnUnlockStateChanged;

    [SerializeField]
    private string title;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private GameObject contentObject;

    [Space]
    [SerializeField]
    private HorizontalMenuListItemModifier horizontalMenuListItemModifier;

    public HorizontalMenuListItemModifier HorizontalMenuListItemModifier => horizontalMenuListItemModifier;
    public string Title => title;
    public Sprite Icon => icon;

    public virtual bool IsUnlocked => true;

    public void HideContent()
    {
        if (contentObject != null)
        {
            contentObject.SetActive(false);
        }
    }

    public void ShowContent()
    {
        if (contentObject != null)
        {
            contentObject.SetActive(true);
        }
    }

    protected void InvokeUnlockStateChangedEvent()
    {
        OnUnlockStateChanged?.Invoke();
    }
}

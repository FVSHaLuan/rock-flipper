using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TabButton : MonoBehaviourWithInit
{
    [SerializeField]
    private GameObject tab;

    private Button button;
    private TabGroup tabGroup;
    private bool isActive;
    private bool isLocked;

    /// <summary>
    /// A locked tab button can't be clicked, and its tab group won't activate it.
    /// </summary>
    public bool IsLocked
    {
        get => isLocked;
        set
        {
            ///
            TryInit();

            ///
            isLocked = value;
            ViewInteractable();
        }
    }

    protected override bool Init()
    {
        button = GetComponent<Button>();
        tabGroup = GetComponentInParent<TabGroup>();

        ///
        return base.Init();
    }

    public void HandleClick()
    {
        ///
        TryInit();

        ///        
        if (tabGroup != null)
        {
            tabGroup.SetActiveTab(this);
        }
    }

    public void SetActive(bool isActive)
    {
        ///
        TryInit();

        ///
        this.isActive = isActive;
        ViewInteractable();
        tab.SetActive(isActive);
    }

    private void ViewInteractable()
    {
        button.interactable = !isActive && !isLocked;
    }
}

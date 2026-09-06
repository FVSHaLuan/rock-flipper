using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TabGroup : MonoBehaviourWithInit
{
    [SerializeField]
    private TabButton defaultTab;

    private List<TabButton> tabButtons;

    protected override bool Init()
    {
        tabButtons = new List<TabButton>();
        GetComponentsInChildren(tabButtons);

        ///
        SetActiveTab(defaultTab);

        ///
        return base.Init();
    }

    public void SetActiveTab(TabButton tabButton)
    {
        foreach (var button in tabButtons)
        {
            button.SetActive(button == tabButton);
        }
    }
}

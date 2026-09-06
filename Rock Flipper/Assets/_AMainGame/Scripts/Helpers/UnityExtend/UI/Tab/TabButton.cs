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
        button.interactable = !isActive;
        tab.SetActive(isActive);
    }
}

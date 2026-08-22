using System.Collections.Generic;
using UnityEngine;

public class GeneralList : MonoBehaviourWithInit
{
    [SerializeField]
    private GeneralListItem firstItem;

    private int count;
    private List<GeneralListItem> listItems;

    public int Count => count;

    protected override bool Init()
    {
        ///
        listItems = new List<GeneralListItem>();
        listItems.Add(firstItem);

        ///
        firstItem.gameObject.SetActive(false);

        ///
        return base.Init();
    }

    [ContextMenu("Clear"), PlayModeOnly]
    public void Clear()
    {
        ///
        TryInit();

        ///
        count = 0;

        ///
        foreach (var item in listItems)
        {
            item.gameObject.SetActive(false);
        }
    }

    [ContextMenu("AddItem")]
    public GeneralListItem AddItem()
    {
        ///
        TryInit();

        ///
        count++;

        ///
        while (listItems.Count < count)
        {
            var listItem = Instantiate(firstItem, firstItem.transform.parent);
            listItem.gameObject.SetActive(false);
            listItem.transform.SetAsLastSibling();
            listItems.Add(listItem);
        }

        ///
        var rs = listItems[count - 1];
        rs.gameObject.SetActive(true);

        ///
        return rs;
    }
}

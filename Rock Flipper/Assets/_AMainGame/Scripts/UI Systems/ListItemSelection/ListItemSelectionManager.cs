using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItemSelectionManager : MonoBehaviour
{
    public event System.Action OnSelectedItemChanged;

    [SerializeField]
    private RectTransform viewRectTransform;
    [SerializeField]
    private bool forceUpdateEveryFrame = true;

    private List<ListItemWrapper> listItemWrappers = new List<ListItemWrapper>();

    private ListItemSelectableRect selectedItem;
    private int selectedItemRegistrationId;

    private bool listItemWrappersDirtyFlag = false;

    private Vector3[] viewWorldCorners = new Vector3[4];
    private Rect viewRect;

    private int lastRegistrationId = -1;

    public ListItemSelectableRect SelectedItem
    {
        get => selectedItem;
        private set
        {
            ///
            if (selectedItem == value)
            {
                return;
            }

            ///
            selectedItem = value;
            selectedItemRegistrationId = selectedItem != null ? selectedItem.RegistrationId : -1;

            ///
            OnSelectedItemChanged?.Invoke();
        }
    }

    public InputDirection InputDirectionFlag { get; set; } = InputDirection.None;

    public enum InputDirection
    {
        None,
        Next,
        Previous
    }

    [System.Serializable]
    private struct ListItemWrapper
    {
        private static Vector3[] worldCorners = new Vector3[4];

        public ListItemSelectableRect item;
        public RectTransform rectTransform;
        public Vector2 bottomLeft;
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomRight;
        public bool isVisible;

        public ListItemWrapper(ListItemSelectableRect listItemSelectable)
        {
            item = listItemSelectable;
            rectTransform = item.GetComponent<RectTransform>();
            bottomLeft = new Vector2();
            topLeft = new Vector2();
            topRight = new Vector2();
            bottomRight = new Vector2();
            isVisible = false;
        }

        public ListItemWrapper UpdateWorldCorners()
        {
            ///
            var rs = this;

            ///
            rectTransform.GetWorldCorners(worldCorners);

            ///
            rs.bottomLeft = worldCorners[0];
            rs.topLeft = worldCorners[1];
            rs.topRight = worldCorners[2];
            rs.bottomRight = worldCorners[3];

            ///
            return rs;
        }

        public ListItemWrapper UpdateVisibility(Rect viewRect)
        {
            ///
            var rs = this;

            ///
            rs.isVisible = viewRect.Contains(bottomLeft) || viewRect.Contains(topLeft) || viewRect.Contains(topRight) || viewRect.Contains(bottomRight);

            ///
            return rs;
        }
    }

    public int RegisterItem(ListItemSelectableRect listItemSelectable)
    {
        ///
        listItemWrappers.Add(new ListItemWrapper(listItemSelectable));

        ///
        listItemWrappersDirtyFlag = true;

        ///
        return ++lastRegistrationId;
    }

    public void UnregisterItem(ListItemSelectableRect listItemSelectable)
    {
        for (int i = 0; i < listItemWrappers.Count; i++)
        {
            if (listItemWrappers[i].item == listItemSelectable)
            {
                ///
                listItemWrappers.RemoveAt(i);

                ///
                listItemWrappersDirtyFlag = true;

                ///
                break;
            }
        }
    }

    public void SetNextInput()
    {
        InputDirectionFlag = InputDirection.Next;
    }

    public void SetPreviousInput()
    {
        InputDirectionFlag = InputDirection.Previous;
    }

    private void UpdateWorldCornersForItem()
    {
        for (int i = 0; i < listItemWrappers.Count; i++)
        {
            listItemWrappers[i] = listItemWrappers[i].UpdateWorldCorners();
        }
    }

    private void UpdateWorldItemsVisibility()
    {
        for (int i = 0; i < listItemWrappers.Count; i++)
        {
            listItemWrappers[i] = listItemWrappers[i].UpdateVisibility(viewRect);
        }
    }

    protected void LateUpdate()
    {
        ///
        if (forceUpdateEveryFrame || listItemWrappersDirtyFlag || InputDirectionFlag != InputDirection.None)
        {
            ///
            UpdateSelectedItem();
        }

        ///
        listItemWrappersDirtyFlag = false;
        InputDirectionFlag = InputDirection.None;
    }

    private void UpdateSelectedItem()
    {
        ///
        UpdateWorldCornersForItem();

        ///
        viewRectTransform.GetWorldCorners(viewWorldCorners);
        viewRect = new Rect(viewWorldCorners[0], viewWorldCorners[2] - viewWorldCorners[0]);

        ///
        UpdateWorldItemsVisibility();
        SortItems();

        ///
        var selectedItemId = FindSelectedItemId();

        ///
        if (selectedItemId < 0)
        {
            ///
            SelectFirstVisibleItem();

            ///
            return;
        }

        ///
        if (!listItemWrappers[selectedItemId].isVisible)
        {
            ///
            if (SelectNextVisibleItem(selectedItemId))
            {
                return;
            }
            else if (SelectPreviousVisibleItem(selectedItemId))
            {
                return;
            }

            ///
            SelectedItem = null;

            ///
            return;
        }

        ///
        if (InputDirectionFlag == InputDirection.None)
        {
            return;
        }

        ///
        switch (InputDirectionFlag)
        {
            case InputDirection.Next:
                SelectNextVisibleItem(selectedItemId);
                break;
            case InputDirection.Previous:
                SelectPreviousVisibleItem(selectedItemId);
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    private bool SelectNextVisibleItem(int selectedItemId)
    {
        return TrySelectItem(selectedItemId + 1);
    }

    private bool TrySelectItem(int itemId)
    {
        ///       
        if (itemId >= listItemWrappers.Count || itemId < 0)
        {
            return false;
        }

        ///
        var wrapper = listItemWrappers[itemId];
        if (wrapper.isVisible)
        {
            ///
            SelectedItem = wrapper.item;

            ///
            return true;
        }

        ///
        return false;
    }

    private bool SelectPreviousVisibleItem(int selectedItemId)
    {
        return TrySelectItem(selectedItemId - 1);
    }

    private void SelectFirstVisibleItem()
    {
        ///
        for (int i = 0; i < listItemWrappers.Count; i++)
        {
            var wrapper = listItemWrappers[i];
            if (wrapper.isVisible)
            {
                ///
                SelectedItem = wrapper.item;

                ///
                return;
            }
        }

        ///
        SelectedItem = null;
    }

    private int FindSelectedItemId()
    {
        ///
        if (selectedItem == null)
        {
            return -1;
        }

        ///
        for (int i = 0; i < listItemWrappers.Count; i++)
        {
            var item = listItemWrappers[i].item;
            if (item == selectedItem && item.RegistrationId == selectedItemRegistrationId)
            {
                return i;
            }
        }

        ///
        return -1;
    }

    private void SortItems()
    {
        listItemWrappers.Sort(ItemVerticalComparison);
    }

    private int ItemVerticalComparison(ListItemWrapper x, ListItemWrapper y)
    {
        ///
        var fx = x.topLeft.y;
        var fy = y.topLeft.y;

        ///
        if (fx == fy)
        {
            return 0;
        }
        else if (fx > fy)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}

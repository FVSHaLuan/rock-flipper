using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shows <see cref="content"/> (the "red dot" explanation and the dismiss-all button) on the Items tab while at least
    /// one item is unseen (<see cref="ItemState.unseen"/>). The button marks every item seen.
    /// </summary>
    public class UnseenItemsNotice : ExtendedMonoBehaviourRun
    {
        [SerializeField, Tooltip("Must not be this GameObject or one of its parents: it's deactivated while no item is unseen.")]
        private GameObject content;
        [SerializeField]
        private Button markAllSeenButton;

        private RunData subscribedRunData;

        protected override void ExtendedAwake()
        {
            base.ExtendedAwake();
            markAllSeenButton.onClick.AddListener(MarkAllSeenButton_OnClick);
        }

        protected void OnEnable()
        {
            ///
            subscribedRunData = RunData;
            subscribedRunData.OnItemStateChanged += RunData_OnItemStateChanged;

            ///
            UpdateView();
        }

        protected void OnDisable()
        {
            // RunData outlives this scene, so drop the handler even if RunEntry is already destroyed.
            if (subscribedRunData != null)
            {
                subscribedRunData.OnItemStateChanged -= RunData_OnItemStateChanged;
                subscribedRunData = null;
            }
        }

        private void RunData_OnItemStateChanged(ItemState itemState)
        {
            UpdateView();
        }

        private void MarkAllSeenButton_OnClick()
        {
            RunData.MarkAllItemsSeen();
        }

        private void UpdateView()
        {
            content.SetActive(subscribedRunData.HasUnseenItems);
        }
    }

}

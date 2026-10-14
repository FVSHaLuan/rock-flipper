using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shows a notice badge on the Items tab button while at least one item is unseen (<see cref="ItemState.unseen"/>).
    /// </summary>
    public class ItemsTabButtonNoticeBadge : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameObject badge;

        private RunData subscribedRunData;

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

        private void UpdateView()
        {
            badge.SetActive(subscribedRunData.HasUnseenItems);
        }
    }

}

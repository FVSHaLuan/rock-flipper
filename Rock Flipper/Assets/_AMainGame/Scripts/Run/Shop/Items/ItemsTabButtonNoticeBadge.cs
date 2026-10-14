using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Shows a notice badge on the Items tab button while at least one item is unseen (<see cref="ItemState.unseen"/>),
    /// counting a found item only once it lands on the button (<see cref="FoundItemAnimation.HasRevealedUnseenItems"/>).
    /// </summary>
    public class ItemsTabButtonNoticeBadge : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameObject badge;

        private RunData subscribedRunData;
        private FoundItemAnimation subscribedFoundItemAnimation;

        protected void OnEnable()
        {
            ///
            subscribedRunData = RunData;
            subscribedRunData.OnItemStateChanged += RunData_OnItemStateChanged;
            subscribedFoundItemAnimation = RunEntry.foundItemAnimation;
            subscribedFoundItemAnimation.OnItemRevealed += FoundItemAnimation_OnItemRevealed;

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
            if (subscribedFoundItemAnimation != null)
            {
                subscribedFoundItemAnimation.OnItemRevealed -= FoundItemAnimation_OnItemRevealed;
                subscribedFoundItemAnimation = null;
            }
        }

        private void RunData_OnItemStateChanged(ItemState itemState)
        {
            UpdateView();
        }

        private void FoundItemAnimation_OnItemRevealed(string itemId)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            badge.SetActive(subscribedFoundItemAnimation.HasRevealedUnseenItems);
        }
    }

}

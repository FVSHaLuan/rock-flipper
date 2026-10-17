using Agame.Run;
using System;

namespace Agame
{
    /// <summary>
    /// Cash each item has earned (<see cref="Run.Shop.ItemState.earnedCash"/>), shown in its tooltip.
    /// </summary>
    public partial class RunData
    {
        /// <summary>
        /// Adds <paramref name="amount"/> to the cash <paramref name="itemId"/> has earned.
        /// Only records it: the cash itself is added with <see cref="AddCurrency(Currency, double)"/>.
        /// </summary>
        public void AddItemEarnedCash(string itemId, double amount)
        {
            ///
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Earned cash to add cannot be negative.");
            }

            ///
            var itemState = GetItemState(itemId);
            itemState.earnedCash += amount;
            SetItemState(itemId, itemState);
        }
    }

}

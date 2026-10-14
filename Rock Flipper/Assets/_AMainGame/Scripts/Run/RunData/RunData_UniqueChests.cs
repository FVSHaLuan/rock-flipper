using System.Collections.Generic;
using UnityEngine;

namespace Agame
{
    /// <summary>
    /// Which backgrounds have dropped their Unique chest. See <see cref="Run.Combat.UniqueChestDropRules"/>.
    /// </summary>
    public partial class RunData
    {
        [SerializeField]
        private List<int> uniqueChestDroppedBackgroundIds = new List<int>();

        /// <summary>How many Unique chests have dropped in this run, one per background at most.</summary>
        public int UniqueChestDropCount => uniqueChestDroppedBackgroundIds == null ? 0 : uniqueChestDroppedBackgroundIds.Count;

        public bool HasBackgroundDroppedUniqueChest(int backgroundId)
        {
            return uniqueChestDroppedBackgroundIds != null && uniqueChestDroppedBackgroundIds.Contains(backgroundId);
        }

        public void MarkBackgroundDroppedUniqueChest(int backgroundId)
        {
            ///
            if (uniqueChestDroppedBackgroundIds == null)
            {
                uniqueChestDroppedBackgroundIds = new List<int>();
            }

            ///
            if (!uniqueChestDroppedBackgroundIds.Contains(backgroundId))
            {
                uniqueChestDroppedBackgroundIds.Add(backgroundId);
            }
        }
    }

}

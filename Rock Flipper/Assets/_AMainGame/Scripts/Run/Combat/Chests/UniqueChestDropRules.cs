using Agame.FeatureBranching;
using Agame.Run.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Decides the weight of the Unique rarity when a chest drops (<see cref="ChestUtilities.TryPickRarity"/>), and which
    /// Unique item a dropped Unique chest holds (<see cref="TryCreateUniqueChestState"/>).
    /// A Unique chest can only drop when all of these hold, otherwise its weight is 0:
    /// <list type="bullet">
    /// <item>the background feature is enabled;</item>
    /// <item>the active background hasn't dropped its Unique chest yet;</item>
    /// <item>some non-placeholder Unique item is unfound and not held by a Unique chest on the playfield;</item>
    /// <item>the active background's total active time (<see cref="RunData.GetBackgroundActiveTime"/>) is at least <see cref="minActiveTime"/>;</item>
    /// <item>in demo and playtest builds, no Unique chest has dropped yet in this save.</item>
    /// </list>
    /// Once eligible, the weight starts at the Unique <see cref="ChestRarityBuildStats.weight"/> and grows by
    /// <see cref="weightIncrease"/> for every full <see cref="weightIncreaseInterval"/> of active time past <see cref="minActiveTime"/>.
    /// Also starts counting background active time once the background feature unlocks.
    /// </summary>
    public class UniqueChestDropRules : ExtendedMonoBehaviourRun
    {
        [SerializeField, Tooltip("Seconds added to the active background's (grassland's) active time when the background feature unlocks. Negative delays it.")]
        private float firstBackgroundTimeOffset = 0;
        [SerializeField, Min(0), Tooltip("Seconds of play time a background must have been active in total before it can drop its Unique chest.")]
        private float minActiveTime = 300;
        [SerializeField, Min(1), Tooltip("Seconds of active time, past the min active time, between two Unique weight increases.")]
        private float weightIncreaseInterval = 120;
        [SerializeField, Min(0), Tooltip("Added to the Unique chest weight every weight increase interval.")]
        private float weightIncrease = 10;

        private readonly HashSet<string> heldUniqueItemIds = new HashSet<string>();

        protected void Start()
        {
            // Subscribed in Start, after RunEntry's first build stats rebuild, so RunData's play time is valid here.
            BuildStatsObject.OnEnabledBackgrounds += BuildStatsObject_OnEnabledBackgrounds;
            TryStartBackgroundActiveTime();
        }

        protected void OnDestroy()
        {
            BuildStatsObject.OnEnabledBackgrounds -= BuildStatsObject_OnEnabledBackgrounds;
        }

        private void BuildStatsObject_OnEnabledBackgrounds()
        {
            TryStartBackgroundActiveTime();
        }

        private void TryStartBackgroundActiveTime()
        {
            if (BuildStats.BackgroundsEnabled)
            {
                RunData.StartBackgroundActiveTime(firstBackgroundTimeOffset);
            }
        }

        /// <summary>
        /// The weight of the Unique rarity for a chest dropping right now. 0 when a Unique chest can't drop.
        /// </summary>
        public float GetUniqueChestWeight()
        {
            ///
            if (!CanDropUniqueChest())
            {
                return 0;
            }

            ///
            var runData = RunData;
            var activeTime = runData.GetBackgroundActiveTime(runData.ActiveBackgroundId);
            if (activeTime < minActiveTime)
            {
                return 0;
            }

            ///
            var baseWeight = BuildStats.GetChestRarityBuildStats(ChestRarity.Unique).weight;
            var increaseCount = Mathf.FloorToInt((activeTime - minActiveTime) / weightIncreaseInterval);
            return baseWeight + increaseCount * weightIncrease;
        }

        /// <summary>
        /// Call when a Unique chest drops: picks the Unique item it holds, records that the active background dropped
        /// its Unique chest, and returns the state to spawn the chest with.
        /// Returns false, recording nothing, if no Unique item is left to hold.
        /// </summary>
        public bool TryCreateUniqueChestState(out ChestState state)
        {
            ///
            var runData = RunData;
            CollectHeldUniqueItemIds();
            var item = RunEntry.itemManager.GetRandomItem(ChestRarity.Unique, runData, heldUniqueItemIds);
            heldUniqueItemIds.Clear();
            if (item == null)
            {
                state = default;
                return false;
            }

            ///
            var backgroundId = runData.ActiveBackgroundId;
            runData.MarkBackgroundDroppedUniqueChest(backgroundId);
            state = new ChestState
            {
                rarity = ChestRarity.Unique,
                uniqueItemId = item.ItemId,
                uniqueBackgroundId = backgroundId,
            };
            return true;
        }

        private bool CanDropUniqueChest()
        {
            ///
            if (!BuildStats.BackgroundsEnabled || !RunData.StartedBackgroundActiveTime)
            {
                return false;
            }

            // Saved chests may hold Unique items, so wait until they are back on the playfield.
            var chestInstanceManager = RunEntry.chestInstanceManager;
            if (!chestInstanceManager.SpawnedSavedChests)
            {
                return false;
            }

            ///
            var runData = RunData;
            if (runData.HasBackgroundDroppedUniqueChest(runData.ActiveBackgroundId))
            {
                return false;
            }

            // Demo and playtest builds drop only 1 Unique chest per save.
            if (VersionBranchInfo.IsPlaytestOrDemo && runData.UniqueChestDropCount > 0)
            {
                return false;
            }

            ///
            CollectHeldUniqueItemIds();
            var itemsLeft = RunEntry.itemManager.GetDroppableItemCount(ChestRarity.Unique, runData, heldUniqueItemIds);
            heldUniqueItemIds.Clear();
            return itemsLeft > 0;
        }

        /// <summary>
        /// Fills <see cref="heldUniqueItemIds"/> with the items held by the Unique chests on the playfield.
        /// </summary>
        private void CollectHeldUniqueItemIds()
        {
            heldUniqueItemIds.Clear();
            foreach (var chest in RunEntry.chestInstanceManager.ActiveChests)
            {
                if (chest.Rarity == ChestRarity.Unique && !string.IsNullOrEmpty(chest.UniqueItemId))
                {
                    heldUniqueItemIds.Add(chest.UniqueItemId);
                }
            }
        }
    }

}

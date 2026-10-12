using Agame.Run.Combat;
using GD;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class ItemManager : ScriptableObjectWithInit
    {
        [SerializeField]
        private List<Item> itemPrefabs = new List<Item>();

        [System.NonSerialized]
        private Dictionary<ChestRarity, List<Item>> itemListByRarityDictionary;
        [System.NonSerialized]
        private Dictionary<string, Item> itemByIdDictionary;
        [System.NonSerialized]
        private readonly List<Item> droppableItemsBuffer = new List<Item>();

        public int AllItemCount => itemPrefabs.Count;

        protected override void Init()
        {
            ///
#if UNITY_EDITOR
            if (!Validate())
            {
                throw new System.Exception($"Validation failed in ItemManager: {name}");
            }
#endif

            // fill the dictionaries with the item prefabs
            itemListByRarityDictionary = new Dictionary<ChestRarity, List<Item>>();
            itemByIdDictionary = new Dictionary<string, Item>();
            foreach (var item in itemPrefabs)
            {
                // Rarity lists
                if (!itemListByRarityDictionary.ContainsKey(item.Rarity))
                {
                    itemListByRarityDictionary[item.Rarity] = new List<Item>();
                }
                itemListByRarityDictionary[item.Rarity].Add(item);

                // ID dictionary
                itemByIdDictionary[item.ItemId] = item;
            }

            ///
            base.Init();
        }

        public Item GetItemByIndex(int index)
        {
            if (index < 0 || index >= itemPrefabs.Count)
            {
                Debug.LogError($"Index {index} is out of range for itemPrefabs.");
                return null;
            }
            return itemPrefabs[index];
        }

        /// <summary>
        /// Applies every item's current item level to the build stats.
        /// </summary>
        public void ApplyToBuildStats(RunData runData)
        {
            foreach (var item in itemPrefabs)
            {
                var level = runData.GetItemState(item.ItemId).level;
                if (level > 0)
                {
                    item.ApplyToBuildStats(0, level);
                }
            }
        }

        /// <summary>
        /// Picks a random item of <paramref name="rarity"/> that can drop in the current build (<see cref="Item.IsDroppableInCurrentBuild"/>), each with the same chance.
        /// Returns null if there is none.
        /// </summary>
        public Item GetRandomItem(ChestRarity rarity, IRandomGenerator random = null)
        {
            ///
            TryInit();

            ///
            if (!itemListByRarityDictionary.TryGetValue(rarity, out var items))
            {
                throw new System.Exception($"No item with rarity {rarity} in ItemManager: {name}");
            }

            ///
            droppableItemsBuffer.Clear();
            foreach (var item in items)
            {
                if (item.IsDroppableInCurrentBuild)
                {
                    droppableItemsBuffer.Add(item);
                }
            }
            if (droppableItemsBuffer.Count == 0)
            {
                return null;
            }

            ///
            random ??= UnityRandom.Default;
            var pickedItem = droppableItemsBuffer.GetRandomItem(random);
            droppableItemsBuffer.Clear();
            return pickedItem;
        }

        public Item GetItemById(string itemId)
        {
            ///
            TryInit();

            ///
            Item item;
            if (itemByIdDictionary.TryGetValue(itemId, out item))
            {
                return item;
            }
            else
            {
                throw new System.Exception($"Item with ID {itemId} not found in itemByIdDictionary.");
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Whether the item prefab at <paramref name="itemAssetPath"/> is listed in any <see cref="ItemManager"/> asset.
        /// </summary>
        public static bool Editor_IsItemRegistered(string itemAssetPath)
        {
            foreach (var guid in UnityEditor.AssetDatabase.FindAssets($"t:{nameof(ItemManager)}"))
            {
                var manager = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemManager>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
                if (manager == null)
                {
                    continue;
                }
                foreach (var item in manager.itemPrefabs)
                {
                    if (item != null && UnityEditor.AssetDatabase.GetAssetPath(item) == itemAssetPath)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [ContextMenu("Validate")]
        private void Editor_Validate()
        {
            if (Validate())
            {
                Debug.Log($"ItemManager {name} is valid. {itemPrefabs.Count} items.");
            }
        }

        private bool Validate()
        {
            bool isValid = true;

            ///
            HashSet<Item> uniqueItems = new HashSet<Item>();
            Dictionary<string, Item> itemsById = new Dictionary<string, Item>();

            ///
            foreach (var item in itemPrefabs)
            {
                ///
                if (item == null)
                {
                    Debug.LogError($"Item is null in ItemManager: {name}");
                    isValid = false;
                    continue;
                }

                ///
                if (!uniqueItems.Add(item))
                {
                    Debug.LogError($"Duplicate item found in ItemManager: {name}. Item name: {item.name}", item);
                    isValid = false;
                    continue;
                }

                ///
                var originalSource = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(item);
                if (UnityEditor.PrefabUtility.GetPrefabAssetType(item.gameObject) != UnityEditor.PrefabAssetType.Variant
                    || originalSource == null || originalSource.name != Item.ItemBasePrefabName)
                {
                    Debug.LogError($"Item is not a prefab variant of {Item.ItemBasePrefabName} in ItemManager: {name}. Item name: {item.name}", item);
                    isValid = false;
                }

                ///
                if (item.BuildAgent == null && !item.IsPlaceholder)
                {
                    Debug.LogError($"Item has no BuildAgent in ItemManager: {name}. Item name: {item.name}", item);
                    isValid = false;
                }

                ///
                if (string.IsNullOrEmpty(item.ItemId))
                {
                    Debug.LogError($"Item has an empty ID in ItemManager: {name}. Item name: {item.name}", item);
                    isValid = false;
                }
                else if (itemsById.TryGetValue(item.ItemId, out var other))
                {
                    Debug.LogError($"Duplicate item ID found: {item.ItemId} ({other.name}, {item.name}). Each item ID must be unique.", item);
                    isValid = false;
                }
                else
                {
                    itemsById[item.ItemId] = item;
                }
            }

            ///
            return isValid;
        }
#endif
    }

}

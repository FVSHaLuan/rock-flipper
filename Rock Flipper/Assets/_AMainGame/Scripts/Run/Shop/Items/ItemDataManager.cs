using Agame.Run.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class ItemDataManager : ScriptableObjectWithInit
    {
        [SerializeField]
        private List<Item> itemPrefabs = new List<Item>();

        [System.NonSerialized]
        private Dictionary<ChestRarity, List<Item>> itemListByRarityDictionary;
        [System.NonSerialized]
        private Dictionary<string, Item> itemByIdDictionary;

        public int AllItemCount => itemPrefabs.Count;

        protected override void Init()
        {
            ///
#if UNITY_EDITOR
            if (!Validate())
            {
                throw new System.Exception($"Validation failed in ItemDataManager: {name}");
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
        [ContextMenu("Validate")]
        private void Editor_Validate()
        {
            if (Validate())
            {
                Debug.Log($"ItemDataManager {name} is valid. {itemPrefabs.Count} items.");
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
                    Debug.LogError($"Item is null in ItemDataManager: {name}");
                    isValid = false;
                    continue;
                }

                ///
                if (!uniqueItems.Add(item))
                {
                    Debug.LogError($"Duplicate item found in ItemDataManager: {name}. Item name: {item.name}", item);
                    isValid = false;
                    continue;
                }

                ///
                if (string.IsNullOrEmpty(item.ItemId))
                {
                    Debug.LogError($"Item has an empty ID in ItemDataManager: {name}. Item name: {item.name}", item);
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

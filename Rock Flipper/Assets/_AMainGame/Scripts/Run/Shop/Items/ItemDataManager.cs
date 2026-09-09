using Agame.Run.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class ItemDataManager : ScriptableObjectWithInit
    {
        [SerializeField, UnityCustomArrayElementHeader]
        private List<ItemData> itemDataList = new List<ItemData>();

        [System.NonSerialized]
        private Dictionary<ChestRarity, List<ItemData>> itemDataListByRarityDictionary;
        [System.NonSerialized]
        private Dictionary<string, ItemData> itemDataByIdDictionary;

        public int AllItemCount => itemDataList.Count;

        protected override void Init()
        {
            // fill the dictionary with the item data
            itemDataListByRarityDictionary = new Dictionary<ChestRarity, List<ItemData>>();
            itemDataByIdDictionary = new Dictionary<string, ItemData>();
            foreach (var itemData in itemDataList)
            {
                // Rarity lists
                if (!itemDataListByRarityDictionary.ContainsKey(itemData.Rarity))
                {
                    itemDataListByRarityDictionary[itemData.Rarity] = new List<ItemData>();
                }
                itemDataListByRarityDictionary[itemData.Rarity].Add(itemData);

                // ID dictionary
                if (!itemDataByIdDictionary.ContainsKey(itemData.ItemId))
                {
                    itemDataByIdDictionary[itemData.ItemId] = itemData;
                }
                else
                {
                    Debug.LogError($"Duplicate item ID found: {itemData.ItemId}. Each item ID must be unique.");
                }
            }

            ///
            base.Init();
        }

        public ItemData GetItemByIndex(int index)
        {
            if (index < 0 || index >= itemDataList.Count)
            {
                Debug.LogError($"Index {index} is out of range for itemDataList.");
                return null;
            }
            return itemDataList[index];
        }

        public ItemData GetItemById(string itemId)
        {
            ///
            TryInit();

            ///
            ItemData itemData;
            if (itemDataByIdDictionary.TryGetValue(itemId, out itemData))
            {
                return itemData;
            }
            else
            {
                throw new System.Exception($"Item with ID {itemId} not found in itemDataByIdDictionary.");
            }
        }
    }

}
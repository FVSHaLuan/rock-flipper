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
        private Dictionary<ChestRarity, List<ItemData>> itemDataDictionary;
        [System.NonSerialized]
        private Dictionary<string, ItemData> itemDataByIdDictionary;

        public int AllItemCount => itemDataList.Count;

        protected override void Init()
        {
            // fill the dictionary with the item data
            itemDataDictionary = new Dictionary<ChestRarity, List<ItemData>>();
            foreach (var itemData in itemDataList)
            {
                if (!itemDataDictionary.ContainsKey(itemData.Rarity))
                {
                    itemDataDictionary[itemData.Rarity] = new List<ItemData>();
                }
                itemDataDictionary[itemData.Rarity].Add(itemData);
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
using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Shop
{
    [System.Serializable]
    public class ItemData : IUnityCustomArrayElementHeader
    {
        [SerializeField]
        private ChestRarity rarity;
        [SerializeField]
        private string itemId;
        [SerializeField]
        private string itemName;        
        [SerializeField]
        private Sprite itemIcon;

        public string ItemId => itemId;
        public string ItemName => itemName;
        public ChestRarity Rarity => rarity;
        public Sprite ItemIcon => itemIcon;

        string IUnityCustomArrayElementHeader.GetHeader(int index)
        {
            return $"[{index}] - {rarity} - {itemName} - {itemId}";
        }
    }

}
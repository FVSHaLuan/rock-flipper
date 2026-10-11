using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Root component of an item prefab. Each item is its own prefab, registered in <see cref="ItemDataManager"/>.
    /// The prefab is a data holder only; it is never spawned.
    /// </summary>
    public class Item : MonoBehaviour
    {
        public const string ItemIdPrefix = "item_";

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

#if UNITY_EDITOR
        protected void Reset()
        {
            TryAssignNewItemId();
        }

        protected void OnValidate()
        {
            TryAssignNewItemId();
        }

        private void TryAssignNewItemId()
        {
            if (string.IsNullOrEmpty(itemId))
            {
                Editor_AssignNewItemId();
            }
        }

        [ContextMenu("Assign New Item Id")]
        public void Editor_AssignNewItemId()
        {
            ///
            UnityEditor.Undo.RecordObject(this, "Assign New Item Id");

            ///
            itemId = ItemIdPrefix + System.Guid.NewGuid().ToString("N").Substring(0, 8);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

}

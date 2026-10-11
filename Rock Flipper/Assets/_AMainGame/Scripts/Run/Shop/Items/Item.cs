using Agame.Run.Combat;
using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Root component of an item prefab. Each item is its own prefab, registered in <see cref="ItemManager"/>.
    /// The prefab is a data holder only; it is never spawned.
    /// Every production item prefab is a variant of the <see cref="ItemBasePrefabName"/> prefab.
    /// </summary>
    public class Item : MonoBehaviour
    {
        public const string ItemIdPrefix = "item_";
        public const string ItemBasePrefabName = "ItemBase";

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
            // only prefab assets (on disk or open in prefab mode) get an id, never scene instances
            bool isPrefabAsset = UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this)
                || UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null;
            if (!isPrefabAsset)
            {
                return;
            }

            // the base prefab has no source and keeps an empty id;
            // a variant needs its own id, not the one inherited from its source
            var source = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(this);
            if (source == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(itemId) || itemId == source.itemId)
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

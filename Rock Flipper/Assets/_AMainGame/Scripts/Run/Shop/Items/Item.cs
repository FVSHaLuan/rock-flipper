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
            TryAssignItemIdFromName();
        }

        protected void OnValidate()
        {
            TryAssignItemIdFromName();
        }

        /// <summary>
        /// Keeps the id in sync with the item's name while the item is new.
        /// Once the item is registered in an <see cref="ItemManager"/> the id is frozen, so save data stays stable.
        /// </summary>
        private void TryAssignItemIdFromName()
        {
            // only prefab assets (on disk or open in prefab mode) get an id, never scene instances
            string assetPath = Editor_GetPrefabAssetPath();
            if (string.IsNullOrEmpty(assetPath))
            {
                return;
            }

            // the base prefab has no source and keeps an empty id
            var source = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(this);
            if (source == null)
            {
                return;
            }

            string newItemId = Editor_GetItemIdFromName(assetPath);
            if (newItemId == itemId)
            {
                return;
            }

            // a variant needs its own id, not the one inherited from its source
            bool needsId = string.IsNullOrEmpty(itemId) || itemId == source.itemId;
            if (!needsId && ItemManager.Editor_IsItemRegistered(assetPath))
            {
                return;
            }

            Editor_SetItemId(newItemId);
        }

        [ContextMenu("Assign Item Id From Name")]
        public void Editor_AssignItemIdFromName()
        {
            string assetPath = Editor_GetPrefabAssetPath();
            Editor_SetItemId(Editor_GetItemIdFromName(assetPath));
        }

        private void Editor_SetItemId(string newItemId)
        {
            ///
            UnityEditor.Undo.RecordObject(this, "Assign Item Id");

            ///
            itemId = newItemId;
            UnityEditor.EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Path of the prefab asset this component belongs to, whether loaded from disk or open in prefab mode.
        /// Empty for scene instances.
        /// </summary>
        private string Editor_GetPrefabAssetPath()
        {
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
            if (prefabStage != null)
            {
                return prefabStage.assetPath;
            }
            if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
            {
                return UnityEditor.AssetDatabase.GetAssetPath(gameObject);
            }
            return string.Empty;
        }

        /// <summary>
        /// Builds the id from <see cref="itemName"/>, or from the prefab file name if it's empty.
        /// "Lucky Pebble" → "item_lucky_pebble".
        /// </summary>
        private string Editor_GetItemIdFromName(string assetPath)
        {
            string sourceName = !string.IsNullOrWhiteSpace(itemName)
                ? itemName
                : System.IO.Path.GetFileNameWithoutExtension(assetPath);

            var sb = new System.Text.StringBuilder();
            foreach (char c in sourceName.ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
                else if (sb.Length > 0 && sb[sb.Length - 1] != '_')
                {
                    sb.Append('_');
                }
            }
            return ItemIdPrefix + sb.ToString().TrimEnd('_');
        }
#endif
    }

}

using Agame.Balancing;
using Agame.FeatureBranching;
using Agame.Localization;
using Agame.Run.Combat;
using Agame.Run.Stats.Agents;
using UnityEngine;
using UnityEngine.Localization;

namespace Agame.Run.Shop
{
    /// <summary>
    /// Root component of an item prefab. Each item is its own prefab, registered in <see cref="ItemManager"/>.
    /// The prefab is a data holder only; it is never spawned.
    /// Every production item prefab is a variant of the <see cref="ItemBasePrefabName"/> prefab.
    /// The item's effect is the referenced <see cref="Stats.Agents.BuildAgent"/> prefab (the same agent prefabs skill nodes use), applied once per item level with <see cref="BuildValue"/>.
    /// Placeholder items (<see cref="IsPlaceholder"/>) fill the item count per rarity until official items replace them; they have no effect.
    /// </summary>
    public class Item : MonoBehaviour
    {
        public const string ItemIdPrefix = "item_";
        public const string ItemBasePrefabName = "ItemBase";

        [SerializeField]
        [Tooltip("Stands in for a not-yet-designed item. Official items replace placeholders instead of being added next to them.")]
        private bool isPlaceholder;
        [SerializeField]
        [Tooltip("Whether chests can drop this item in demo and playtest builds.")]
        private bool isAvailableInDemo;
        [SerializeField]
        private ChestRarity rarity;
        [SerializeField]
        private string itemId;
        [SerializeField]
        private string itemName;
        [SerializeField, Tooltip("Overrides itemName when set.")]
        private LocalizedString localizedItemName;
        [SerializeField]
        private Sprite itemIcon;
        [SerializeField]
        private BuildAgent buildAgent;
        [SerializeField]
        private double buildValue;

        public bool IsPlaceholder => isPlaceholder;
        public bool IsAvailableInDemo => isAvailableInDemo;
        public string ItemId => itemId;
        public string ItemName => localizedItemName.GetLocalizedStringOr(itemName);
        public ChestRarity Rarity => rarity;
        public Sprite ItemIcon => itemIcon;
        public double BuildValue => buildValue;
        public BuildAgent BuildAgent => buildAgent;

        /// <summary>
        /// Whether chests can drop this item in the current build.
        /// Demo and playtest builds drop neither placeholders nor items not available in the demo.
        /// </summary>
        public bool IsDroppableInCurrentBuild => !VersionBranchInfo.IsPlaytestOrDemo || (!isPlaceholder && isAvailableInDemo);

        /// <summary>
        /// Applies <paramref name="addingLevel"/> item levels, starting from <paramref name="currentLevel"/>, to the build stats.
        /// </summary>
        public void ApplyToBuildStats(int currentLevel, int addingLevel)
        {
            ///
            if (buildAgent == null)
            {
                if (isPlaceholder)
                {
                    return;
                }
                Debug.LogWarning($"Item {name} has no BuildAgent!", this);
                return;
            }

            ///
            buildAgent.Apply(currentLevel, addingLevel, buildValue);
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            TryAssignItemIdFromName();
        }

        protected void OnValidate()
        {
            TryAssignItemIdFromName();
        }

        [ContextMenu("Editor_LogMaxBuildValue")]
        private void Editor_LogMaxBuildValue()
        {
            var maxLevel = GameBalance.GetItemMaxLevel(rarity);
            var maxBuildValue = maxLevel * buildValue;

            ///
            var skillDescriptor = buildAgent != null ? buildAgent.GetComponent<SkillDescriptor>() : null;
            if (skillDescriptor != null)
            {
                Debug.Log($"Max lv. = {maxLevel}, {skillDescriptor.GetDescription(maxBuildValue)}", this);
                return;
            }
            Debug.Log($"Max level = {maxLevel}, max build value = {maxBuildValue}", this);
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
        /// Builds the id from the English text of <see cref="localizedItemName"/>, else from <see cref="itemName"/>,
        /// else from the prefab file name. "Lucky Pebble" → "item_lucky_pebble".
        /// </summary>
        private string Editor_GetItemIdFromName(string assetPath)
        {
            string sourceName = localizedItemName.Editor_GetEnglishString();
            if (string.IsNullOrWhiteSpace(sourceName))
            {
                sourceName = itemName;
            }
            if (string.IsNullOrWhiteSpace(sourceName))
            {
                sourceName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            }

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

using FH.Core.Architecture.Pool;
using TMPro;
using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// A world-space display of an item (its icon and name), e.g. the item a chest drops.
    /// Not part of the UI system. Pooled: any instance can show any item through <see cref="SetItem"/>.
    /// </summary>
    public class ItemAvatar : GeneralPoolMemberSimplified
    {
        [SerializeField]
        private SpriteRenderer iconRenderer;
        [SerializeField]
        private TextMeshPro nameText;

        public Item Item { get; private set; }

        public void SetItem(Item item)
        {
            ///
            Item = item;

            ///
            iconRenderer.sprite = item.ItemIcon;
            nameText.SetText(item.ItemName);
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            iconRenderer = GetComponentInChildren<SpriteRenderer>();
            nameText = GetComponentInChildren<TextMeshPro>();
        }
#endif
    }

}

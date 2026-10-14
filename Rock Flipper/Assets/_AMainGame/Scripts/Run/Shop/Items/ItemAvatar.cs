using Agame.Run.Combat;
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
            iconRenderer.color = item.Rarity.GetForegroundColor();
            FitIconInUnitSquare();
            nameText.SetText(item.ItemName);
            SetNameAlpha(1f);
        }

        /// <summary>
        /// Scales the icon uniformly so its longer side is 1 unit, whatever the sprite's size and pixels per unit.
        /// </summary>
        private void FitIconInUnitSquare()
        {
            var sprite = iconRenderer.sprite;
            if (sprite == null)
            {
                return;
            }

            var spriteSize = sprite.bounds.size;
            var longerSide = Mathf.Max(spriteSize.x, spriteSize.y);
            if (longerSide <= 0f)
            {
                return;
            }

            iconRenderer.transform.localScale = Vector3.one / longerSide;
        }

        /// <summary>
        /// Fades the name in or out. <see cref="SetItem"/> shows it fully again.
        /// </summary>
        public void SetNameAlpha(float alpha)
        {
            nameText.alpha = alpha;
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

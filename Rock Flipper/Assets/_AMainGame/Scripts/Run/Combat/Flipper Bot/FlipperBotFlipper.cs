using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    public class FlipperBotFlipper : ExtendedMonoBehaviourRun
    {
        [SerializeField, Tooltip("Center of the flip range. The range radius is BuildStats.flipperBotFlipRange.")]
        private CircleCollider2D refCollider;
        [SerializeField]
        private UnityEvent onFlipRocks;

        private List<Flippable> castFlippables = new List<Flippable>();
        private List<Flippable> flippables = new List<Flippable>();

        /// <summary>
        /// Center of the flip range circle
        /// </summary>
        public Vector2 FlipRangeCenter => refCollider.bounds.center;

        [ContextMenu("Try To Flip Rocks"), PlayModeOnly]
        public void TryToFlipRocks()
        {
            ///
            FindFlippablesInRange();

            ///
            var flipCount = Mathf.Min(BuildStats.flipperBotStrength, flippables.Count);
            for (int i = 0; i < flipCount; i++)
            {
                // partial Fisher-Yates: pick a random one among the not-yet-picked
                var pickedIndex = Random.Range(i, flippables.Count);
                (flippables[i], flippables[pickedIndex]) = (flippables[pickedIndex], flippables[i]);

                flippables[i].TryFlipping(FlipSource.FlipperBot);
            }

            ///
            onFlipRocks?.Invoke();
        }

        /// <summary>
        /// Fills <see cref="flippables"/> with the distinct flippables in range that can be flipped right now.
        /// </summary>
        private void FindFlippablesInRange()
        {
            SimpleCast2D.CircleCast(FlipRangeCenter, BuildStats.flipperBotFlipRange, true, castFlippables);

            flippables.Clear();
            foreach (var flippable in castFlippables)
            {
                if (flippable.IsFlipping || flippable.IsFlippingLocked || flippables.Contains(flippable))
                {
                    continue;
                }

                if (!BuildStats.flipperBotChestFlippingEnabled && flippable.GetComponentInParent<Chest>() != null)
                {
                    continue;
                }

                flippables.Add(flippable);
            }
        }
    }

}

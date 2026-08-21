using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    public class FlipperBotFlipper : MonoBehaviour
    {
        [SerializeField]
        private CircleCollider2D refCollider;
        [SerializeField]
        private UnityEvent onFlipRocks;

        private List<Flippable> flippables = new List<Flippable>();

        [ContextMenu("Try To Flip Rocks"), PlayModeOnly]
        public void TryToFlipRocks()
        {
            ///
            CastForFlippables();

            ///
            foreach (var flippable in flippables)
            {
                flippable.TryFlipping();
            }

            ///
            onFlipRocks?.Invoke();
        }

        private void CastForFlippables()
        {
            SimpleCast2D.CircleCast(refCollider.bounds.center, refCollider.radius, true, flippables);
        }
    }

}
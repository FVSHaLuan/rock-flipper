using Agame.Run.Combat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Dev
{
    /// <summary>
    /// While its toggle is on: blocks the real player cursor from flipping, and
    /// - until mouse hover is unlocked: simulates clicks by flipping a random rock or chest the cursor could flip, flipsPerSecond times per second
    /// - once mouse hover is unlocked: simulates hovering with a virtual cursor that moves toward a random rock or chest the cursor could flip
    ///   at hoverSpeed, flipping everything under it the way the real cursor would (point or mouse radius)
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class AutoMouse : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private PlayerCursor playerCursor;
        [SerializeField, Min(0f)]
        private float flipsPerSecond = 5f;
        [SerializeField, Min(0f)]
        private float hoverSpeed = 10f;

        private Toggle toggle;
        private bool isLocking;
        private float clickTimer;

        private bool isHovering;
        private Vector2 hoverPosition;
        private FlippableByPlayerCursor hoverTarget;

        private readonly List<FlippableByPlayerCursor> candidates = new List<FlippableByPlayerCursor>();
        private readonly List<FlippableByPlayerCursor> hoverHits = new List<FlippableByPlayerCursor>();

        protected void Start()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
            OnToggleValueChanged(toggle.isOn);
        }

        protected void OnDestroy()
        {
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
            }
            SetLocking(false);
        }

        protected void Update()
        {
            if (toggle == null || !toggle.isOn)
            {
                StopClicking();
                StopHovering();
                return;
            }

            ///
            if (BuildStats.enabledMouseHover)
            {
                StopClicking();
                UpdateHovering();
            }
            else
            {
                StopHovering();
                UpdateClicking();
            }
        }

        private void UpdateClicking()
        {
            clickTimer += Time.deltaTime * flipsPerSecond;
            while (clickTimer >= 1f)
            {
                if (!TryFlippingRandomFlippable())
                {
                    // nothing to flip: click as soon as something is, without banking the missed clicks
                    clickTimer = 1f;
                    return;
                }
                clickTimer -= 1f;
            }
        }

        private void StopClicking()
        {
            clickTimer = 0f;
        }

        private void UpdateHovering()
        {
            if (!isHovering)
            {
                // start from wherever the real cursor is
                isHovering = true;
                hoverPosition = playerCursor != null ? (Vector2)playerCursor.transform.position : Vector2.zero;
            }

            ///
            if (!IsCandidate(hoverTarget))
            {
                hoverTarget = PickRandomCandidate();
            }
            if (hoverTarget != null)
            {
                hoverPosition = Vector2.MoveTowards(hoverPosition, hoverTarget.transform.position, hoverSpeed * Time.deltaTime);
            }

            ///
            if (playerCursor == null)
                return;
            playerCursor.FindHits(hoverPosition, hoverHits);
            foreach (var item in hoverHits)
            {
                if (item.isActiveAndEnabled && item.CooledDown)
                {
                    item.Flippable.TryFlipping(FlipSource.Mouse);
                }
            }
        }

        private void StopHovering()
        {
            isHovering = false;
            hoverTarget = null;
        }

        private bool TryFlippingRandomFlippable()
        {
            var target = PickRandomCandidate();
            return target != null && target.Flippable.TryFlipping(FlipSource.Mouse);
        }

        private FlippableByPlayerCursor PickRandomCandidate()
        {
            CollectFlippables();
            return candidates.Count > 0 ? candidates[Random.Range(0, candidates.Count)] : null;
        }

        private void CollectFlippables()
        {
            candidates.Clear();
            foreach (var rock in RunEntry.rockInstanceManager.ActiveRocks)
            {
                TryAddingCandidate(rock.FlippableByPlayerCursor);
            }
            foreach (var chest in RunEntry.chestInstanceManager.ActiveChests)
            {
                TryAddingCandidate(chest.FlippableByPlayerCursor);
            }
        }

        private void TryAddingCandidate(FlippableByPlayerCursor byCursor)
        {
            if (IsCandidate(byCursor))
                candidates.Add(byCursor);
        }

        /// <summary>
        /// Same checks <see cref="PlayerCursor"/> applies to a flippable under the cursor, plus not already airborne
        /// </summary>
        private static bool IsCandidate(FlippableByPlayerCursor byCursor)
        {
            return byCursor != null && byCursor.isActiveAndEnabled && byCursor.CooledDown && !byCursor.Flippable.IsFlipping;
        }

        private void OnToggleValueChanged(bool isOn)
        {
            SetLocking(isOn);
        }

        private void SetLocking(bool locking)
        {
            if (isLocking == locking || playerCursor == null)
            {
                return;
            }

            ///
            isLocking = locking;
            if (locking)
            {
                playerCursor.AddFlippingLock(this);
            }
            else
            {
                playerCursor.RemoveFlippingLock(this);
            }
        }

        protected void OnDrawGizmos()
        {
            if (!isHovering)
                return;

            ///
            Gizmos.color = Color.cyan;
            var radius = BuildStats.enabledMouseRadius ? BuildStats.mouseRadius : 0.1f;
            Gizmos.DrawWireSphere(hoverPosition, radius);
        }
    }
}

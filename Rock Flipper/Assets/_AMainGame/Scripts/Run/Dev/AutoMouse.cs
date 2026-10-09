using Agame.Run.Combat;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Dev
{
    /// <summary>
    /// While its toggle is on: blocks the real player cursor from flipping, and (until mouse hover is unlocked)
    /// simulates clicks by flipping a random rock or chest the cursor could flip, flipsPerSecond times per second
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class AutoMouse : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private PlayerCursor playerCursor;
        [SerializeField, Min(0f)]
        private float flipsPerSecond = 5f;

        private Toggle toggle;
        private bool isLocking;
        private float clickTimer;

        private readonly List<FlippableByPlayerCursor> candidates = new List<FlippableByPlayerCursor>();

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
            if (toggle == null || !toggle.isOn || BuildStats.enabledMouseHover)
            {
                clickTimer = 0f;
                return;
            }

            ///
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

        private bool TryFlippingRandomFlippable()
        {
            CollectFlippables();
            if (candidates.Count == 0)
                return false;

            ///
            return candidates[Random.Range(0, candidates.Count)].Flippable.TryFlipping(FlipSource.Mouse);
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

        /// <summary>
        /// Same checks <see cref="PlayerCursor"/> applies to a flippable under the cursor, plus not already airborne
        /// </summary>
        private void TryAddingCandidate(FlippableByPlayerCursor byCursor)
        {
            if (byCursor != null && byCursor.isActiveAndEnabled && byCursor.CooledDown && !byCursor.Flippable.IsFlipping)
                candidates.Add(byCursor);
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
    }
}

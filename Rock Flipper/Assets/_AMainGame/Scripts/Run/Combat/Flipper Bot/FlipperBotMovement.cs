using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBotMovement : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private float baseSpeed = 3f;

        [Header("Smart Pathing")]
        [SerializeField, Tooltip("Seconds between two picks of the best spot to go to while Smart Pathing is enabled.")]
        private float smartPathingRetargetInterval = 0.25f;
        [SerializeField, Tooltip("A new spot replaces the current one only if it scores more than this many times the current one's, so the bot doesn't jitter between similar spots.")]
        private float smartPathingSwitchThreshold = 1.2f;

        private float Speed => baseSpeed * BuildStats.flipperBotMovementSpeedFactor;

        private FlipperBot flipperBot;
        private Vector2 currentTarget;
        private bool hasTarget;

        private float smartPathingTimer;
        private readonly List<Vector2> smartPathingClaimedCenters = new List<Vector2>();
        private readonly List<Vector2> smartPathingCandidates = new List<Vector2>();

        /// <summary>
        /// Whether the current target was picked by Smart Pathing (as opposed to random wandering)
        /// </summary>
        public bool HasSmartTarget { get; private set; }
        /// <summary>
        /// Flip range center the bot will have once it reaches its Smart Pathing target
        /// </summary>
        public Vector2 SmartTargetFlipCenter { get; private set; }

        protected override void ExtendedAwake()
        {
            flipperBot = GetComponent<FlipperBot>();
        }

        protected void Update()
        {
            if (flipperBot.State == FlipperBot.FlipperBotState.Active)
            {
                ChaseTarget();
            }
        }

        private void ChaseTarget()
        {
            ///
            if (BuildStats.flipperBotSmartPathingEnabled)
            {
                smartPathingTimer -= Time.deltaTime;
                if (smartPathingTimer <= 0f)
                {
                    smartPathingTimer = smartPathingRetargetInterval;
                    UpdateSmartTarget();
                }
            }
            else if (HasSmartTarget)
            {
                HasSmartTarget = false;
                hasTarget = false;
            }

            ///
            if (!hasTarget)
            {
                currentTarget = GetRandomTarget();
                hasTarget = true;
            }

            ///
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, Speed * Time.deltaTime);

            ///
            // a reached Smart Pathing target is kept, so the bot waits there for its next flip
            if (!HasSmartTarget && (Vector2)transform.position == currentTarget)
            {
                currentTarget = GetRandomTarget();
            }
        }

        private Vector2 GetRandomTarget()
        {
            return Playfield.GetRandomPoint(-Vector2.one * 0.2f);
        }

        #region Smart Pathing
        /// <summary>
        /// Picks the spot with the best flip rate (flippables flipped per second, travel time included).
        /// Candidate spots are centered on each flippable; rocks and chests count the same.
        /// Falls back to random wandering when nothing is left to flip.
        /// </summary>
        private void UpdateSmartTarget()
        {
            ///
            CollectSmartPathingCandidates();

            ///
            var botCenter = flipperBot.Flipper.FlipRangeCenter;
            var bestScore = 0f;
            var bestCenter = Vector2.zero;
            foreach (var center in smartPathingCandidates)
            {
                var score = GetSmartPathingScore(center, botCenter);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestCenter = center;
                }
            }

            ///
            if (bestScore <= 0f)
            {
                if (HasSmartTarget)
                {
                    HasSmartTarget = false;
                    hasTarget = false;
                }

                return;
            }

            ///
            if (HasSmartTarget && bestScore <= GetSmartPathingScore(SmartTargetFlipCenter, botCenter) * smartPathingSwitchThreshold)
                return;

            ///
            HasSmartTarget = true;
            SmartTargetFlipCenter = bestCenter;
            // move the bot so that its flip range center (not its pivot) lands on the spot
            currentTarget = bestCenter - (botCenter - (Vector2)transform.position);
            hasTarget = true;
        }

        /// <summary>
        /// Fills <see cref="smartPathingCandidates"/> with the positions of the flippables this bot could flip,
        /// minus those another Active bot is already heading for
        /// </summary>
        private void CollectSmartPathingCandidates()
        {
            ///
            smartPathingClaimedCenters.Clear();
            foreach (var otherBot in RunEntry.flipperBotInstanceManager.ActiveFlipperBots)
            {
                if (otherBot == flipperBot || otherBot.State != FlipperBot.FlipperBotState.Active)
                    continue;

                var otherMovement = otherBot.Movement;
                if (otherMovement != null && otherMovement.HasSmartTarget)
                {
                    smartPathingClaimedCenters.Add(otherMovement.SmartTargetFlipCenter);
                }
            }

            ///
            smartPathingCandidates.Clear();
            foreach (var rock in RunEntry.rockInstanceManager.ActiveRocks)
            {
                TryAddSmartPathingCandidate(rock.Flippable);
            }

            if (BuildStats.flipperBotChestFlippingEnabled)
            {
                foreach (var chest in RunEntry.chestInstanceManager.ActiveChests)
                {
                    TryAddSmartPathingCandidate(chest.Flippable);
                }
            }
        }

        private void TryAddSmartPathingCandidate(Flippable flippable)
        {
            ///
            if (flippable.IsFlipping || flippable.IsFlippingLocked)
                return;

            ///
            var position = (Vector2)flippable.transform.position;
            var sqrRange = BuildStats.flipperBotFlipRange * BuildStats.flipperBotFlipRange;
            foreach (var claimedCenter in smartPathingClaimedCenters)
            {
                if ((position - claimedCenter).sqrMagnitude <= sqrRange)
                    return;
            }

            ///
            smartPathingCandidates.Add(position);
        }

        /// <summary>
        /// Flippables flipped per second when going to <paramref name="center"/> and flipping there once:
        /// the candidates within flip range (capped by strength), over travel time + flip interval
        /// </summary>
        private float GetSmartPathingScore(Vector2 center, Vector2 botCenter)
        {
            ///
            var sqrRange = BuildStats.flipperBotFlipRange * BuildStats.flipperBotFlipRange;
            var count = 0;
            foreach (var candidate in smartPathingCandidates)
            {
                if ((candidate - center).sqrMagnitude <= sqrRange)
                    count++;
            }

            count = Mathf.Min(count, BuildStats.flipperBotStrength);
            if (count <= 0)
                return 0f;

            ///
            var travelTime = Vector2.Distance(botCenter, center) / Mathf.Max(Speed, 0.01f);
            return count / (travelTime + BuildStats.flipperBotFlippingInterval);
        }
        #endregion
    }

}

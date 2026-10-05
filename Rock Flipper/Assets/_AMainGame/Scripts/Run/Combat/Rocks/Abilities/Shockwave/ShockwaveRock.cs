using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Gives the rock a chance to trigger a shockwave after it lands (and survives the landing),
    /// flipping up to a max number of random rocks on the ground within a radius around it
    /// (a landing from a shockwave flip can trigger a shockwave too, so chain reactions are possible)
    /// </summary>
    public class ShockwaveRock : RockExtender
    {
        [SerializeField]
        private UnityEvent onShockwave;

        private readonly List<Rock> rockHits = new List<Rock>();
        private readonly List<Rock> candidates = new List<Rock>();

        protected void Start()
        {
            Rock.OnLanded += Rock_OnLanded;
        }

        private void Rock_OnLanded()
        {
            if (Random.value >= BuildStats.shockwaveRockShockwaveChance)
                return;

            ///
            TriggerShockwave();
        }

        private void TriggerShockwave()
        {
            ///
            CastForGroundRocks();

            ///
            var flipCount = Mathf.Min(BuildStats.shockwaveRockMaxShockwaveFlips, candidates.Count);
            for (int i = 0; i < flipCount; i++)
            {
                // partial Fisher-Yates shuffle: pick a random remaining candidate
                var j = Random.Range(i, candidates.Count);
                (candidates[i], candidates[j]) = (candidates[j], candidates[i]);

                ///
                candidates[i].Flippable.TryFlipping(FlipSource.Shockwave);
            }

            ///
            onShockwave?.Invoke();
        }

        private void CastForGroundRocks()
        {
            ///
            SimpleCast2D.CircleCast(Rock.transform.position, BuildStats.shockwaveRockShockwaveRadius, true, rockHits);

            ///
            candidates.Clear();
            foreach (var rock in rockHits)
            {
                if (rock == Rock || rock.Flippable.IsFlipping || candidates.Contains(rock))
                    continue;

                ///
                candidates.Add(rock);
            }
        }
    }

}

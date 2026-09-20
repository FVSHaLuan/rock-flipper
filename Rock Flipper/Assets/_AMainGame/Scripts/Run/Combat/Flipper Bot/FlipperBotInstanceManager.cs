using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBotInstanceManager : ExtendedMonoBehaviourRun
    {
        /// <summary>
        /// Flipper Bots that are currently active in the scene, regardless they are "active" or "idle" in terms of their behavior. This list is used to keep track of all Flipper Bots that have been spawned and are currently present in the game world.
        /// </summary>
        private List<FlipperBot> activeFlipperBots = new List<FlipperBot>();

        public int ActiveFlipperBotCount => activeFlipperBots.Count;

        [ContextMenu("Spawn Flipper Bot"), PlayModeOnly]
        public FlipperBot SpawnFlipperBot()
        {
            return SpawnFlipperBot(RunEntry.prototypeManager.FlipperBotPrototype);
        }

        public FlipperBot SpawnFlipperBot(FlipperBotPoolHandler flipperBotPoolHandler)
        {
            var flipperBot = CurrentGeneralPool.TakeInstance(flipperBotPoolHandler, this).TargetObject;

            ///
            flipperBot.transform.position = Playfield.GetRandomPoint(-Vector2.one);
            flipperBot.gameObject.SetActive(true);

            ///
            activeFlipperBots.Add(flipperBot);

            ///
            return flipperBot;
        }
    }

}
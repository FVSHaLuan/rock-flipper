using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBotInstanceManager : ExtendedMonoBehaviourRun
    {
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
            return flipperBot;
        }
    }

}
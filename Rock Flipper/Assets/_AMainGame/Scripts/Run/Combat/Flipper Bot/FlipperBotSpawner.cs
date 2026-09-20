using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBotSpawner : ExtendedMonoBehaviourRun
    {
        protected void Update()
        {
            var spawnCount = BuildStats.flipperBotCount - RunEntry.flipperBotInstanceManager.ActiveFlipperBotCount;
            for (int i = 0; i < spawnCount; i++)
            {
                RunEntry.flipperBotInstanceManager.SpawnFlipperBot();
            }
        }
    }

}
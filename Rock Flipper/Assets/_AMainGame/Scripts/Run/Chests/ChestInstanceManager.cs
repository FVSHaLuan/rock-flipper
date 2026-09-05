using UnityEngine;

namespace Agame.Run.Combat
{
    public class ChestInstanceManager : ExtendedMonoBehaviourRun
    {
        public Chest Spawn(ChestRarity chestRarity, ChestState? state = null)
        {
            var chestPrototype = RunEntry.prototypeManager.GetChestPrototype(chestRarity);
            var chest = CurrentSceneGeneralPool.TakeInstance(chestPrototype.PoolHandler, this).TargetObject;
            if (state.HasValue)
            {
                chest.ApplyState(state.Value);
            }
            chest.transform.position = Playfield.GetRandomPoint(-Vector2.one);
            chest.gameObject.SetActive(true);
            return chest;
        }
    }
}
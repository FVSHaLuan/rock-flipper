using UnityEngine;

namespace BT.Run.Combat
{
    public class InitRunState : ExtendedMonoBehaviourRun
    {
        protected void Start()
        {
            if (RunData.IsInPrestige)
            {
                StateManager.StartPrestigeImmediately();
            }
            else
            {
                StateManager.StartCombat();
            }
        }
    }

}
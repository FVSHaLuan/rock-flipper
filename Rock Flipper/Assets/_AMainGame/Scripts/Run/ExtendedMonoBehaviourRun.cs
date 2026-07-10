using BT.Run.Combat;
using BT.Run.Stats;
using UnityEngine;

namespace BT.Run
{
    public class ExtendedMonoBehaviourRun : ExtendedMonoBehaviour
    {
        protected RunEntry RunEntry => RunEntry.Instance;
        protected RunStateManager StateManager => RunEntry.runStateManager;
        protected RunState CurrentRunState => RunEntry.runStateManager.CurrentState;
        protected GeneralPool CurrentGeneralPool => RunEntry.GeneralPool;
        protected RunData RunData => RunEntry.RunData;
        protected BuildStatsObject BuildStats => RunEntry.BuildStats;
        protected Playfield Playfield => RunEntry.playfield;
        protected bool IsTutorial => !RunData.FinishedTutorial;

        private int lastInitedCombat = -1;

        public void TryInitThisCombat()
        {
            TryInitThisCombatBool();
        }

        public bool TryInitThisCombatBool()
        {
            ///
            if (CurrentRunState != RunState.Combat)
            {
                return false;
            }

            ///
            if (lastInitedCombat == StateManager.CombatId)
            {
                return false;
            }

            ///
            lastInitedCombat = StateManager.CombatId;
            InitThisCombat();

            ///
            return true;
        }

        protected virtual void InitThisCombat() { }
    }

}
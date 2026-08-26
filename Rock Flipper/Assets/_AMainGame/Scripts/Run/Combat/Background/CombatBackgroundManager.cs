using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat.Backgrounds
{
    public class CombatBackgroundManager : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private List<CombatBackground> combatBackgrounds = new List<CombatBackground>();

        private CombatBackground activeBackground;

        protected override bool Init()
        {
            ///
            return base.Init();
        }

        protected void Start()
        {
            if (CurrentRunState == RunState.Combat)
            {
                UpdateBackgroundForCurrentCombat();
            }

            ///
            StateManager.OnCombatStarted += StateManager_OnCombatStarted;
        }

        public CombatBackground GetBackground(int id)
        {
            return combatBackgrounds[id];
        }

        public int GetBackgroundId(CombatBackground combatBackground)
        {
            return combatBackgrounds.IndexOf(combatBackground);
        }

        private void StateManager_OnCombatStarted()
        {
            UpdateBackgroundForCurrentCombat();
        }

        public void UpdateBackgroundForCurrentCombat()
        {
            var background = combatBackgrounds[RunData.ActiveBackgroundId];
            SetActiveBackground(background);
        }

        private void SetActiveBackground(CombatBackground combatBackground)
        {
            ///
            activeBackground = combatBackground;

            ///
            foreach (var background in combatBackgrounds)
            {
                background.gameObject.SetActive(background == combatBackground);
            }

            ///
            RunEntry.gameplayCamera.backgroundColor = combatBackground.CameraColor;
            combatBackground.TryInitThisCombat();
        }
    }
}
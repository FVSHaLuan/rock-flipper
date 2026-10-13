using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat.Backgrounds
{
    public class CombatBackgroundManager : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private List<CombatBackground> combatBackgrounds = new List<CombatBackground>();

        private CombatBackground activeBackground;
        private RunData subscribedRunData;

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
            subscribedRunData = RunData;
            subscribedRunData.OnActiveBackgroundIdChanged += RunData_OnActiveBackgroundIdChanged;
        }

        protected void OnDestroy()
        {
            // RunData outlives this scene, so drop the handler even if RunEntry is already destroyed.
            if (subscribedRunData != null)
            {
                subscribedRunData.OnActiveBackgroundIdChanged -= RunData_OnActiveBackgroundIdChanged;
            }
        }

        /// <summary>
        /// Whether the background with this id has a <see cref="CombatBackground"/> in the scene. Ids range from 0 to
        /// <see cref="GameConst.BackgroundCount"/> minus 1, whether or not their background exists; only check this
        /// before accessing the background itself (e.g. its name).
        /// </summary>
        public bool HasBackground(int id)
        {
            return id >= 0 && id < GameConst.BackgroundCount && id < combatBackgrounds.Count && combatBackgrounds[id] != null;
        }

        /// <summary>Returns the background with this id, or background 0 (with a warning) if there's none.</summary>
        public CombatBackground GetBackground(int id)
        {
            if (!HasBackground(id))
            {
                Debug.LogWarning($"Background {id} is missing, falling back to background 0.");
                return combatBackgrounds[0];
            }

            ///
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

        private void RunData_OnActiveBackgroundIdChanged(int backgroundId)
        {
            if (CurrentRunState == RunState.Combat)
            {
                UpdateBackgroundForCurrentCombat();
            }
        }

        public void UpdateBackgroundForCurrentCombat()
        {
            var background = GetBackground(RunData.ActiveBackgroundId);
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
            RunEntry.clearingCamera.backgroundColor = combatBackground.CameraColor;
            combatBackground.TryInitThisCombat();
        }
    }
}
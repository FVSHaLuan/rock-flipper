using Agame.GamePlatform;
using Agame.Run.Combat;
using Agame.Run.Dev;
using System;
using System.Collections;
using UnityEngine;

namespace Agame.Run
{
    public class RunStateManager : ExtendedMonoBehaviourRun
    {
        public event StateChangeHandler OnStateChanged;
        public event System.Action OnCombatStarted;
        public event System.Action OnBeforePrestige;
        public event System.Action OnPrestige;
        public event System.Action OnBeforeCombatFromPrestige;

        public delegate void StateChangeHandler(RunState oldState, RunState currentState);

        [SerializeField]
        private float prestigeDelay = 0.1f;
        [SerializeField]
        private float combatFromPrestigeDelay = 0.1f;

        [Space]
        [SerializeField]
        private AchievementConfig prestigeAchievement;

        private int combatCount;

        public int CombatCount => combatCount;
        public int CombatId => combatCount;
        public RunState CurrentState { get; private set; } = RunState.Init;

        public void StartCombat()
        {
            if (CurrentState != RunState.Init
                && CurrentState != RunState.OnPrestige
                && CurrentState != RunState.BeforeCombatFromPrestige)
            {
                throw new System.InvalidOperationException();
            }

            ///
            if (RunEntry.IsBuildStatsInvalid)
            {
                RunEntry.RebuildBuildStats();
            }

            ///
            combatCount++;

            ///            
            ChangeToState(RunState.Combat);
        }

        public void StartPrestige(Func<bool> releaseFunction)
        {
            StartCoroutine(PrestigeDelay(releaseFunction));
        }

        public void FinishPrestige()
        {
            StartCoroutine(FinishPrestigeDelay());
        }

        public void StartPrestigeImmediately()
        {
            ///
            if (CurrentRunState != RunState.Init
                && CurrentRunState != RunState.BeforePrestige)
            {
                throw new System.InvalidOperationException();
            }

            ///
            // RunData.Prestige();

            ///
            ChangeToState(RunState.OnPrestige);

            ///
            prestigeAchievement.Report();
        }

        private IEnumerator PrestigeDelay(Func<bool> releaseFunction)
        {
            ///
            if (CurrentState != RunState.Combat)
            {
                throw new System.InvalidOperationException();
            }

            ///
            ChangeToState(RunState.BeforePrestige);
            if (releaseFunction == null)
            {
                yield return new WaitForSecondsRealtime(prestigeDelay);
            }
            else
            {
                while (!releaseFunction())
                {
                    yield return null;
                }
            }

            ///
            StartPrestigeImmediately();
        }

        private IEnumerator FinishPrestigeDelay()
        {
            ///
            if (CurrentState != RunState.OnPrestige)
            {
                throw new System.InvalidOperationException();
            }

            ///
            RunLogger.Log("Prestiged");

            ///            
            RunEntry.MarkBuildStatsAsInvalid(true);

            ///
            // RunData.FinishPrestige();
            ChangeToState(RunState.BeforeCombatFromPrestige);
            yield return new WaitForSecondsRealtime(combatFromPrestigeDelay);
            StartCombat();
        }

        private void ChangeToState(RunState newState)
        {
            var savedState = CurrentState;
            CurrentState = newState;

            ///
            OnStateChanged?.Invoke(savedState, newState);

            ///
            switch (newState)
            {
                case RunState.Init:
                    break;
                case RunState.Combat:
                    OnCombatStarted?.Invoke();
                    break;
                case RunState.BeforePrestige:
                    OnBeforePrestige?.Invoke();
                    break;
                case RunState.OnPrestige:
                    OnPrestige?.Invoke();
                    break;
                case RunState.BeforeCombatFromPrestige:
                    OnBeforeCombatFromPrestige?.Invoke();
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}
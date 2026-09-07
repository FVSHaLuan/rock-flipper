using GD;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class LevelProgressView : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private ProgressBar progressBar;

        protected void Start()
        {
            UpdateProgressBar();
        }

        protected void Update()
        {
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            var nextLevelRequirement = gameBalance.GetRequiredExpForNextLevel(RunData.Level);
            var currentExp = RunData.LevelExp;
            progressBar.SetValue((float)(currentExp / nextLevelRequirement));
        }
    }

}
using GD;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class ChestLevelProgressView : ExtendedMonoBehaviourRun
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
            var nextLevelRequirement = gameBalance.GetRequiredExpForNextChestLevel(RunData.ChestLevel);
            var currentExp = RunData.ChestLevelExp;
            progressBar.SetValue((float)(currentExp / nextLevelRequirement));
        }
    }

}
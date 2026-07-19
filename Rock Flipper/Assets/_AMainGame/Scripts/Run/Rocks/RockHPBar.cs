using GD;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class RockHPBar : RockExtender
    {
        [SerializeField]
        private GameObject wrapper;
        [SerializeField]
        private ProgressBar hpProgressBar;

        protected void Start()
        {
            UpdateProgressBarView();

            ///
            Rock.OnHPChanged += Rock_OnHPChanged;
            Rock.OnStartedNewLife += Rock_OnStartedNewLife;
        }

        private void Rock_OnStartedNewLife()
        {
            UpdateProgressBarView();
        }

        private void Rock_OnHPChanged()
        {
            UpdateProgressBarView();
        }

        private void UpdateProgressBarView()
        {
            if (Rock.CurrentHP == Rock.MaxHP)
            {
                wrapper.SetActive(false);
            }
            else
            {
                wrapper.SetActive(true);
                hpProgressBar.SetValue((float)Rock.CurrentHP / Rock.MaxHP);
            }
        }
    }

}
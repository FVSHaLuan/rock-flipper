using GD;
using System;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class ChestHPBar : MonoBehaviour
    {
        [SerializeField]
        private Chest chest;
        [SerializeField]
        private GameObject wrapper;
        [SerializeField]
        private ProgressBar hpProgressBar;

        protected void Start()
        {
            UpdateProgressBarView();
         
            ///
            chest.OnHPChanged += Chest_OnHPChanged;
            chest.OnStartedNewLife += Chest_OnStartedNewLife;
        }

        private void Chest_OnStartedNewLife()
        {
            UpdateProgressBarView();
        }

        private void Chest_OnHPChanged()
        {
            UpdateProgressBarView();
        }

        private void UpdateProgressBarView()
        {
            if (chest.CurrentHP == chest.MaxHP)
            {
                wrapper.SetActive(false);
            }
            else
            {
                wrapper.SetActive(true);
                hpProgressBar.SetValue((float)chest.CurrentHP / chest.MaxHP);
            }
        }
    }

}
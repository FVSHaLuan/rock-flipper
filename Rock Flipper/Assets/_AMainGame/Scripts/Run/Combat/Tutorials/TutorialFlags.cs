using UnityEngine;

namespace Agame.Run.Combat.Tutorials
{
    public class TutorialFlags : ExtendedMonoBehaviourRun
    {
        public event System.Action OnValueChanged;

        [SerializeField]
        private bool enableBlowMode;
        [SerializeField]
        private bool enableEnergyBar;
        [SerializeField]
        private bool enableEnergyBarHighlight;
        [SerializeField]
        private bool enableSpecialCrusher;
        [SerializeField]
        private bool enablePrestigeBar;
        [SerializeField]
        private bool enablePrestigeBarHighlight;

        public bool EnableBlowMode
        {
            get => IsTutorial ? enableBlowMode : true;
            set
            {
                enableBlowMode = value;
                OnValueChanged?.Invoke();
            }
        }
        public bool EnableEnergyBar
        {
            get => IsTutorial ? enableEnergyBar : true;
            set
            {
                enableEnergyBar = value;
                OnValueChanged?.Invoke();
            }
        }
        public bool EnableEnergyBarHighlight
        {
            get => IsTutorial ? enableEnergyBarHighlight : false;
            set
            {
                enableEnergyBarHighlight = value;
                OnValueChanged?.Invoke();
            }
        }
        public bool EnablePrestigeBar
        {
            get => IsTutorial ? enablePrestigeBar : true;
            set
            {
                enablePrestigeBar = value;
                OnValueChanged?.Invoke();
            }
        }
        public bool EnableSpecialCrusher
        {
            get => IsTutorial ? enableSpecialCrusher : true;
            set
            {
                enableSpecialCrusher = value;
                OnValueChanged?.Invoke();
            }
        }
        public bool EnablePrestigeBarHighlight
        {
            get => IsTutorial ? enablePrestigeBarHighlight : false;
            set
            {
                enablePrestigeBarHighlight = value;
                OnValueChanged?.Invoke();
            }
        }
    }

}
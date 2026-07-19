using UnityEngine;

namespace Agame.Run.Combat
{
    public class Rock : MonoBehaviour
    {
        public event System.Action OnStartedNewLife;
        public event System.Action OnHPChanged;

        [SerializeField]
        private int baseHP = 5;

        // [Header("Components")]        

        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }

        [ContextMenu("Start New Life"), PlayModeOnly]
        public void StartNewLife()
        {
            MaxHP = baseHP;
            CurrentHP = MaxHP;

            ///
            OnStartedNewLife?.Invoke();
        }
    }

}
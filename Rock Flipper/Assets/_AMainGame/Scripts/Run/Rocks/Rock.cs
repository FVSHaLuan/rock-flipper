using UnityEngine;

namespace Agame.Run.Combat
{
    public class Rock : MonoBehaviour
    {
        [SerializeField]
        private int baseHP = 5;

        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }

        public void StartNewLife()
        {
            MaxHP = baseHP;
            CurrentHP = MaxHP;
        }
    }

}
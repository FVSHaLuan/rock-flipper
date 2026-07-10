using UnityEngine;

namespace BT.Run
{
    public class CompatManager : ScriptableObjectWithInit
    {
        [SerializeField]
        private int currentCompatVersion = 1;

        public int CurrentCompatVersion => currentCompatVersion;
    }

}
using UnityEngine;

namespace Agame.Run
{
    [System.Serializable]
    public struct CurrencyState
    {
        [SerializeField]
        public int level;
        [SerializeField, Range(0, 1)]
        public int autoSellPercentage;
    }

}
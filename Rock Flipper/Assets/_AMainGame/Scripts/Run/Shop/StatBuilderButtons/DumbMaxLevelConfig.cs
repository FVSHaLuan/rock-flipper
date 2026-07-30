using UnityEngine;

namespace Agame.Run.Shop
{
    public class DumbMaxLevelConfig : MonoBehaviour, IMaxLevelConfig
    {
        [SerializeField]
        private int maxLevel = 1;

        public int MaxLevel => maxLevel;
    }

}

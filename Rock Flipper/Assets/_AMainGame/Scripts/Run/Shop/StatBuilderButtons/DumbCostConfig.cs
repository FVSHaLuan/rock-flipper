using UnityEngine;

namespace Agame.Run.Shop
{
    public class DumbCostConfig : MonoBehaviour, ICostConfig
    {
        [SerializeField]
        private double dumbCost = 1.0;

        public double GetCost(int level)
        {
            return dumbCost;
        }
    }

}
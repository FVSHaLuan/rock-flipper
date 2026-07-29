using UnityEngine;

namespace Agame.Run.Shop
{
    public class DumbBuildValueConfig : MonoBehaviour, IBuildValueConfig
    {
        [SerializeField]
        private double buildValue;

        public double BuildValue => buildValue;
    }

}
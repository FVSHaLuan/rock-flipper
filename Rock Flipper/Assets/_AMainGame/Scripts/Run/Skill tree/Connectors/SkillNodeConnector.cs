using UnityEngine;

namespace Agame.Run
{
    public class SkillNodeConnector : MonoBehaviour
    {
        [SerializeField]
        private Direction8 direction;

        public Direction8 Direction => direction;
    }

}
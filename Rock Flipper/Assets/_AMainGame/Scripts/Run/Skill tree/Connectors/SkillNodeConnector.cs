using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run
{
    public class SkillNodeConnector : MonoBehaviour
    {
        [SerializeField]
        private Direction8 direction;

        [SerializeField]
        private Image image;

        public Direction8 Direction => direction;

        public void SetColor(Color color)
        {
            if (image != null)
            {
                image.color = color;
            }
        }
    }

}
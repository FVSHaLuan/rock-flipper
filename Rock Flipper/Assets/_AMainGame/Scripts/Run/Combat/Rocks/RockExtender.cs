using UnityEngine;

namespace Agame.Run.Combat
{
    public class RockExtender : MonoBehaviour
    {
        private Rock rock;

        protected Rock Rock
        {
            get
            {
                if (rock == null)
                    rock = GetComponentInParent<Rock>();
                return rock;
            }
        }
    }

}
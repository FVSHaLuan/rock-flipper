using UnityEngine;

namespace Agame.Run.Combat
{
    [RequireComponent(typeof(Flippable))]
    public class FlippableByPlayerCursor : MonoBehaviour
    {
        private Flippable flippable;

        public Flippable Flippable
        {
            get
            {
                if (flippable == null)
                {
                    flippable = GetComponent<Flippable>();
                }
                return flippable;
            }
        }
    }

}
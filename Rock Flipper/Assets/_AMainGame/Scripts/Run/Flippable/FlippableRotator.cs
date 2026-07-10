using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlippableRotator : MonoBehaviour
    {
        private Flippable flippable;

        protected void Start()
        {
            var flippable = GetComponentInParent<Flippable>();
            flippable.OnUpdatedFlipping += Flippable_OnUpdatedFlipping;
        }

        private void Flippable_OnUpdatedFlipping()
        {
            
        }
    }

}
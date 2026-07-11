using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlippableRotator : MonoBehaviour
    {
        [SerializeField]
        private float cycleCount = 10;

        private Flippable flippable;

        protected void Start()
        {
            flippable = GetComponentInParent<Flippable>();
            flippable.OnUpdatedFlipping += Flippable_OnUpdatedFlipping;
        }

        private void Flippable_OnUpdatedFlipping()
        {
            var cycle = Mathg.DecimalPart(flippable.FlippingHeightProgress * cycleCount / 2);
            var angle = Mathf.Lerp(0, 360, cycle);
            var th = transformHandle;
            th.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

}
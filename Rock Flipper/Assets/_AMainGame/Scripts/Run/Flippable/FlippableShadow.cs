using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlippableShadow : MonoBehaviour
    {
        [SerializeField]
        private Transform shadow;
        [SerializeField]
        private float minScale = 0.1f;

        private Flippable flippable;

        protected void Start()
        {
            flippable = GetComponentInParent<Flippable>();
            flippable.OnUpdatedFlipping += Flippable_OnUpdatedFlipping;
        }

        private void Flippable_OnUpdatedFlipping()
        {
            var scale = Mathf.Lerp(1, minScale, flippable.HeightProgress);
            shadow.localScale = Vector3.one * scale;
        }
    }
}
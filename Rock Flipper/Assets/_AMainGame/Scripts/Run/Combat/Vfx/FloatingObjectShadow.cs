using UnityEngine;

namespace Agame.Run.Combat.Vfx
{
    public class FloatingObjectShadow : MonoBehaviour
    {
        [SerializeField]
        private Transform shadowTransform;
        [SerializeField]
        private Transform objectTransform;
        [SerializeField]
        private float shadowScalePerDistance = 0.2f;

        private void Update()
        {
            if (shadowTransform == null || objectTransform == null)
                return;

            // Calculate the distance between the object and the shadow
            float distance = 0 - objectTransform.localPosition.y;

            // Scale the shadow based on the distance
            float scale = 1f + (distance * shadowScalePerDistance);
            shadowTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
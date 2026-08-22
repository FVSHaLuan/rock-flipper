using UnityEngine;

public class FloatingObjectShadow : MonoBehaviour
{
    [SerializeField]
    private Transform shadowTransform;
    [SerializeField]
    private Transform objectTransform;
    [SerializeField]
    private float shadowScalePerDistance = 3f;

    protected void Update()
    {
        if (shadowTransform == null || objectTransform == null)
            return;

        // Calculate the distance between the object and the shadow
        float distance = 0 - objectTransform.localPosition.y;

        // Scale the shadow based on the distance
        float scale = 1f + (distance * shadowScalePerDistance);
        if (scale < 0f) scale = 0f; // Prevent negative scale
        shadowTransform.localScale = new Vector3(scale, scale, scale);
    }
}

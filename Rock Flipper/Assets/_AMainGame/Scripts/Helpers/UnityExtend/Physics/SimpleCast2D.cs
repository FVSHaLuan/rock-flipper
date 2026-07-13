using System.Collections.Generic;
using UnityEngine;

public static class SimpleCast2D
{
    private const float PointCastRadius = 0.001f;

    private static RaycastHit2D[] raycastHit2Ds = new RaycastHit2D[100];

    public static void PointCast<T>(Vector2 point, bool isComponentInParent, List<T> results) where T : Component
    {
        // set default layer mask to everything
        PointCast(point, isComponentInParent, Physics2D.AllLayers, results);
    }

    public static void PointCast<T>(Vector2 point, bool isComponentInParent, LayerMask layerMask, List<T> results) where T : Component
    {
        CircleCast(point, PointCastRadius, isComponentInParent, layerMask, results);
    }

    public static void CircleCast<T>(Vector2 center, float radius, bool isComponentInParent, List<T> results) where T : Component
    {
        // set default layer mask to everything
        CircleCast(center, radius, isComponentInParent, Physics2D.AllLayers, results);
    }

    public static void CircleCast<T>(Vector2 center, float radius, bool isComponentInParent, LayerMask layerMask, List<T> results) where T : Component
    {
        ///
        results.Clear();

        ///
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(layerMask);

        ///
        var hitCount = Physics2D.CircleCast(center, radius, Vector2.zero, contactFilter, raycastHit2Ds, 0);
        for (int i = 0; i < hitCount; i++)
        {
            var hit = raycastHit2Ds[i];
            var component = isComponentInParent ? hit.collider.GetComponentInParent<T>() : hit.collider.GetComponent<T>();
            if (component != null)
            {
                results.Add(component);
            }
        }
    }
}

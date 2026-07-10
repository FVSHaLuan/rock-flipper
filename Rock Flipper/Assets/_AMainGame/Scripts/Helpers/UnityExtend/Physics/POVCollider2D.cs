using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

[RequireComponent(typeof(PolygonCollider2D))]
public class POVCollider2D : MonoBehaviour
{
    private PolygonCollider2D polygonCollider2D;
    private Vector2[] points = new Vector2[3];

    public PolygonCollider2D PolygonCollider2D
    {
        get
        {
            ///
            if (polygonCollider2D == null)
            {
                polygonCollider2D = GetComponent<PolygonCollider2D>();
            }

            ///
            return polygonCollider2D;
        }
    }

    /// <summary>
    /// Angles are in degree
    /// </summary>
    /// <param name="range"></param>
    /// <param name="angle"></param>
    /// <param name="rotation"></param>
    public void SetPOV(float range, float angle, float rotation)
    {
        ///
        Assert.IsTrue(angle < 180);

        ///        
        var baseAngleRad = (180 - angle) / 2.0f * Mathf.Deg2Rad;
        var legLength = range / Mathf.Sin(baseAngleRad);
        var halfBaseLength = Mathf.Cos(baseAngleRad) * legLength;

        ///
        var midPoint = new Vector2(0, range);
        var leftCorner = midPoint + new Vector2(-halfBaseLength, 0);
        var rightCorner = midPoint + new Vector2(halfBaseLength, 0);

        ///
        points[0] = Vector3.zero;
        points[1] = leftCorner;
        points[2] = rightCorner;

        ///
        PolygonCollider2D.SetPath(0, points);

        ///
        transform.localEulerAngles = new Vector3(0, 0, rotation - 90);
    }
}

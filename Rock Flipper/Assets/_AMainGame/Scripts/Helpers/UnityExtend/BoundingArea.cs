using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingArea : MonoBehaviour
{
    [SerializeField]
    private Vector2 center;
    [SerializeField]
    private Vector2 size = Vector2.one * 3;
    [SerializeField]
    private bool isLocal = true;

    public Vector2 GetRandomInsidePositionFor(Vector2 objectSize, bool returnWorldSpace = true)
    {
        ///
        var localPos = GetRandomInsidePositionFor(center, size, objectSize);

        ///
        if (isLocal && returnWorldSpace)
        {
            return transform.TransformPoint(localPos);
        }
        else
        {
            return localPos;
        }
    }

    public static Vector2 GetRandomInsidePositionFor(Vector2 center, Vector2 size, Vector2 objectSize)
    {
        ///
        var halfSizeDifference = (size - objectSize) / 2.0f;

        ///
        var minX = center.x - halfSizeDifference.x;
        var maxX = center.x + halfSizeDifference.x;
        var minY = center.y - halfSizeDifference.y;
        var maxY = center.y + halfSizeDifference.y;

        ///
        var x = Random.Range(minX, maxX);
        var y = Random.Range(minY, maxY);

        ///
        var localPos = new Vector2(x, y);

        ///
        return localPos;
    }

    public static void GetRandomPointsInGrid(Vector2 gridCenter, Vector2 gridSize, int colCount, int rowCount, Vector2 objectSize, List<Vector2> points)
    {
        ///
        points.Clear();

        ///    
        var cellSize = new Vector2()
        {
            x = gridSize.x / colCount,
            y = gridSize.y / rowCount
        };

        ///
        var minCellCenterX = gridCenter.x - gridSize.x / 2.0f + cellSize.x / 2.0f;
        var maxCellCenterY = gridCenter.y + gridSize.y / 2.0f - cellSize.y / 2.0f;

        ///
        Vector2 currentCenter = new Vector2(minCellCenterX, maxCellCenterY);
        for (int row = 0; row < rowCount; row++)
        {
            ///
            for (int col = 0; col < colCount; col++)
            {
                ///
                var pos = GetRandomInsidePositionFor(currentCenter, cellSize, objectSize);
                points.Add(pos);

                ///
                currentCenter.x += cellSize.x;
            }

            ///
            currentCenter.x = minCellCenterX;
            currentCenter.y -= cellSize.y;
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        ///
        if (isLocal)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        }

        ///
        Gizmos.color = Color.blue;

        ///
        Gizmos.DrawWireCube(center, size);
    }
#endif
}

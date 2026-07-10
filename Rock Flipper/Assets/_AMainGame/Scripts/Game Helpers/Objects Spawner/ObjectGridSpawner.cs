using FH.Core.Architecture.Pool;
using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGridSpawner : ObjectsSpawner
{
    [Header("ObjectGridSpawner")]
    [SerializeField]
    private Vector2 center;
    [SerializeField]
    private Vector2 gridSize = Vector2.one * 5;
    [SerializeField]
    private Vector2 cellContentSize = Vector2.one * 0.8f;
    [SerializeField]
    private int columnCount = 5;
    [SerializeField]
    private int rowCount = 5;

    public Vector2 Center { get => center; set => center = value; }
    public Vector2 GridSize { get => gridSize; set => gridSize = value; }
    public int ColumnCount { get => columnCount; set => columnCount = value; }
    public int RowCount { get => rowCount; set => rowCount = value; }

    private List<Vector2> availableCells = new List<Vector2>();

    private float minCellCenterX;
    private float maxCellCenterY;
    private Vector2 cellSize;
    private Vector2 halfSizeDifference;

    public int TotalCellToSpawnPreset => (int)((columnCount * rowCount) * objectCountPortion);

    [ContextMenu("GeneratePresetOneFrame")]
    public void GeneratePresetOneFrame()
    {
        foreach (var item in GenerateCells(objectCountPortion, objectPrototype))
        {
            item.gameObject.SetActive(true);
        }
    }

    protected override IEnumerable<GeneralPoolMemberSimplified> GenerateCells(float cellCountPortion, int prototypeId)
    {
        PrepareForGeneration();

        ///
        int cellCount = (int)((columnCount * rowCount) * cellCountPortion);

        ///
        availableCells.Clear();
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                availableCells.Add(new Vector2(i, j));
            }
        }

        ///
        for (int i = 0; i < cellCount; i++)
        {
            ///
            var cellGridPos = availableCells.TakeRandomItem();

            ///
            yield return SpawnCell(cellGridPos, prototypeId);
        }
    }

    public void PrepareForGeneration()
    {
        ///       
        cellSize = new Vector2()
        {
            x = gridSize.x / ColumnCount,
            y = gridSize.y / RowCount
        };

        ///
        minCellCenterX = center.x - gridSize.x / 2.0f + cellSize.x / 2.0f;
        maxCellCenterY = center.y + gridSize.y / 2.0f - cellSize.y / 2.0f;

        ///
        halfSizeDifference = (cellSize - cellContentSize) / 2.0f;
    }

    public GeneralPoolMemberSimplified SpawnCell(Vector2 cellGridPos, int prototypeId)
    {
        ///
        Vector2 cellCenter = new Vector2()
        {
            x = minCellCenterX + cellGridPos.x * cellSize.x,
            y = maxCellCenterY - cellGridPos.y * cellSize.y
        };

        ///
        Vector2 cellPos = new Vector2()
        {
            x = Random.Range(cellCenter.x - halfSizeDifference.x, cellCenter.x + halfSizeDifference.x),
            y = Random.Range(cellCenter.y - halfSizeDifference.y, cellCenter.y + halfSizeDifference.y)
        };

        ///
        var cell = generalPool.TakeInstance(prototypeId, this);

        ///
        cell.transform.SetParent(objectsParent);
        cell.transform.localEulerAngles = Vector3.zero;
        cell.transform.localPosition = cellPos;

        ///
        return cell;
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        ///
        SetGizmosMatrix();

        ///
        cellSize = new Vector2()
        {
            x = gridSize.x / ColumnCount,
            y = gridSize.y / RowCount
        };

        ///
        var minX = center.x - gridSize.x / 2.0f;
        var maxX = center.x + gridSize.x / 2.0f;
        var minY = center.y - gridSize.y / 2.0f;
        var maxY = center.y + gridSize.y / 2.0f;

        ///
        Gizmos.color = Color.blue;

        // Draw grid X
        for (int i = 0; i < columnCount + 1; i++)
        {
            var x = minX + i * cellSize.x;
            var from = new Vector2(x, maxY);
            var to = new Vector2(x, minY);
            Gizmos.DrawLine(from, to);
        }

        // Draw grid Y
        for (int i = 0; i < rowCount + 1; i++)
        {
            var y = maxY - i * cellSize.y;
            var from = new Vector2(minX, y);
            var to = new Vector2(maxX, y);
            Gizmos.DrawLine(from, to);
        }

        ///
        minCellCenterX = center.x - gridSize.x / 2.0f + cellSize.x / 2.0f;
        maxCellCenterY = center.y + gridSize.y / 2.0f - cellSize.y / 2.0f;

        // Draw cell content
        Gizmos.color = Color.red;
        for (int col = 0; col < ColumnCount; col++)
        {
            for (int row = 0; row < RowCount; row++)
            {
                ///
                Vector2 cellCenter = new Vector2()
                {
                    x = minCellCenterX + col * cellSize.x,
                    y = maxCellCenterY - row * cellSize.y
                };

                ///
                Gizmos.DrawWireCube(cellCenter, cellContentSize);
            }
        }

    }

    public void Reset()
    {
        objectsParent = transform;
    }
#endif
}

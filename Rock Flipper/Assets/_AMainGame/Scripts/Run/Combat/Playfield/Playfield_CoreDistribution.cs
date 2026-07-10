using GD;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public partial class Playfield
    {
        private List<Vector2Int> allCoreGridPositions = new List<Vector2Int>();

        public void GetCorePositions(int coreCount, float coreRadius, float paddingX, float paddingY, List<Vector2> results)
        {
            TryInit();

            // Define inner bounds (after padding)
            float innerMinX = MinX + paddingX;
            float innerMaxX = MaxX - paddingX;
            float innerMinY = MinY + paddingY;
            float innerMaxY = MaxY - paddingY;

            float innerWidth = innerMaxX - innerMinX;
            float innerHeight = innerMaxY - innerMinY;

            ///
            var maxColCount = Mathf.FloorToInt(innerWidth / coreRadius);
            var maxRowCount = Mathf.FloorToInt(innerHeight / coreRadius);

            ///
            int colCount = 1;
            int rowCount = 1;
            while (colCount * rowCount < coreCount)
            {
                if (colCount <= rowCount && colCount < maxColCount)
                {
                    colCount++;
                }
                else if (rowCount < maxRowCount)
                {
                    rowCount++;
                }
                else
                {
                    throw new System.Exception("Cannot fit more cores in playfield");
                }
            }

            // fill allCoreGridPositions
            allCoreGridPositions.Clear();
            for (int col = 0; col < colCount; col++)
            {
                for (int row = 0; row < rowCount; row++)
                {
                    allCoreGridPositions.Add(new Vector2Int(col, row));
                }
            }

            ///
            float cellWidth = innerWidth / colCount;
            float cellHeight = innerHeight / rowCount;

            float maxOffsetX = (cellWidth - 2 * coreRadius) * 0.5f;
            float maxOffsetY = (cellHeight - 2 * coreRadius) * 0.5f;

            ///
            results.Clear();

            ///
            for (int i = 0; i < coreCount; i++)
            {
                var gridPos = allCoreGridPositions.TakeRandomItem();

                float x = innerMinX + (gridPos.x + 0.5f) * cellWidth + Random.Range(-maxOffsetX, maxOffsetX);
                float y = innerMaxY - (gridPos.y + 0.5f) * cellHeight + Random.Range(-maxOffsetY, maxOffsetY);

                ///
                results.Add(new Vector2(x, y));
            }
        }
    }
}
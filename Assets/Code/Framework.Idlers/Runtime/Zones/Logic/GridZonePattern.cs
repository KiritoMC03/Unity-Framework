using System;
using UnityEngine;

namespace Framework.Idlers.Zones
{
    public class GridZonePattern
    {
        #region Fields

        private Preferences prefs;

        #endregion

        #region Methods

        public GridZonePattern(Preferences prefs)
        {
            this.prefs = prefs;
        }
        
        /// <summary>
        /// return: Position relative to center in preferences.
        /// </summary>
        public Vector3 GetPosition(int index)
        {
            var totalItemsInRows = prefs.xRowSize * prefs.zRowSize;
            var indexByZ = CalculateIndexInRow(index % totalItemsInRows, totalItemsInRows, prefs.xRowSize);
            var indexByY = Mathf.FloorToInt((float) index / totalItemsInRows);
            var indexByX = index % prefs.xRowSize;

            var result = new Vector3
            {
                x = CalculatePositionInRow(indexByX, prefs.cellSize.x, prefs.cellOffset.x),
                y = indexByY * (prefs.cellSize.y + prefs.cellOffset.y),
                z = CalculatePositionInRow(indexByZ, prefs.cellSize.z, prefs.cellOffset.z)
            };

            result.x -= CalculateOffsetToCenter(prefs.xRowSize, prefs.cellSize.x, prefs.cellOffset.x); 
            result.z -= CalculateOffsetToCenter(prefs.zRowSize, prefs.cellSize.z, prefs.cellOffset.z); 

            return prefs.center.TransformPoint(result);
        }

        private float CalculateOffsetToCenter(int rowSize, float cellSize, float cellOffset) =>
            rowSize * (cellSize + cellOffset) / 2f;

        private float CalculatePositionInRow(int indexInRow, float size, float offset) =>
            (indexInRow + 0.5f) * (offset + size);

        private static int CalculateIndexInRow(int itemIndex, int totalItemsInRows, int otherRowSize)
        {
            if (itemIndex < 0 || itemIndex > totalItemsInRows - 1) return -1;
            return Mathf.FloorToInt((float) itemIndex / otherRowSize);
        }

        #endregion

        [Serializable]
        public struct Preferences
        {
            public Transform center;
            public Vector3 cellSize;
            public Vector3 cellOffset;
            public Vector3 pivotOffset;
            public int xRowSize;
            public int zRowSize;
        }
    }
}
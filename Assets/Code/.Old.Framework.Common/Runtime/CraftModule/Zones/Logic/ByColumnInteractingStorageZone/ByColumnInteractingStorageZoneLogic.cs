using System.Collections.Generic;
using General.Extensions;
using UnityEngine;

namespace GameKit.CraftModule.Zones
{
    public class ByColumnInteractingStorageZoneLogic<TContent> : BaseStorageZoneLogic<TContent>
    {
        #region Fields
        
        /// <summary>
        /// Dictionary, where Key - column position, Value - column (array of cells)
        /// </summary>
        private Dictionary<Vector3, StorageZoneColumn<TContent>> columnsDictionary;
        private List<StorageZoneColumnCell<TContent>> containedObjects;
        private List<Vector3> containedObjectsPositions;
        private List<Vector3> columnsPositions;
        private GridZonePattern pattern;
        private GridZonePattern.Preferences preferences;
        private Vector3 cachedResourcesAccepterPose;
        private Vector3 cachedNearestColumnPose;
        private int objectsNumber;

        private readonly Transform resourcesAccepterTransform;
        private readonly Vector3 defaultNearestColumnPose;
        private readonly float defaultMaxDistanceToColumn;
        private readonly float popRadius;
        private readonly int defaultColumnHeight;

        #endregion

        #region Properties

        /// <summary>
        /// O(1)
        /// </summary>
        public override int ObjectsCount => objectsNumber;
        public override bool CanReplenish => objectsNumber < Capacity;
        public override bool HasObject => objectsNumber > 0;
        public List<StorageZoneColumnCell<TContent>> ContainedCells => containedObjects;
        
        /// <summary>
        /// O(n) Expensive property call. Return the COPY of contained object array.
        /// </summary>
        public override List<TContent> ContainedObjects
        {
            get
            {
                List<TContent> result = new List<TContent>(containedObjects.Count);
                for (int i = 0; i < containedObjects.Count; i++) result[i] = containedObjects[i].content;
                return result;
            }
        }

        #endregion

        #region Constructors

        public ByColumnInteractingStorageZoneLogic(int capacity, 
            float popRadius, 
            GridZonePattern.Preferences preferences, 
            ref List<StorageZoneColumnCell<TContent>> cellsList, 
            Transform resourcesAccepterTransform, 
            int defaultColumnHeight = 10)
        {
            this.preferences = preferences;
            this.resourcesAccepterTransform = resourcesAccepterTransform;
            this.defaultColumnHeight = defaultColumnHeight;
            this.popRadius = popRadius;
            base.Capacity = capacity;
            this.objectsNumber = 0;
            this.defaultNearestColumnPose = Vector3.positiveInfinity;
            this.defaultMaxDistanceToColumn = float.MaxValue;
            this.pattern = new GridZonePattern(preferences);
            GenerateStructures(capacity, ref cellsList);
            RefreshPatternInternal(new GridZonePattern(preferences));
            isInitialized = true;
        }

        #endregion

        #region Override Methods

        public override bool TryReplenish(TContent target)
        {
            if (!CheckInitialized() || !CanReplenish) return false;
            
            bool isFluentlyCellFound = containedObjects.TryFindFluentlyCellIndex(Capacity, out int index); 
            if (!isFluentlyCellFound) return false;
            
            containedObjects[index].content = target;
            objectsNumber++;
            InvokeReplenishedCallback();
            return true;

        }
        
        public override bool TryPopObject(out TContent result)
        {
            result = default;
            if (!CheckInitialized() || objectsNumber < 1) return false;
            
            bool searchingSuccess = TrySearchNearestNotFluentlyColumn(out cachedNearestColumnPose, resourcesAccepterTransform.position);
            if (!searchingSuccess) return false;

            StorageZoneColumn<TContent> nearestColumn = columnsDictionary[cachedNearestColumnPose];
            bool popSuccess = nearestColumn.TryPopTopObject(out result);
            if (!popSuccess) return false;
            objectsNumber--;
            InvokeSentCallback();
            return true;
        }

        public bool TryPopLastObject(out TContent result)
        {
            result = default;
            if (!CheckInitialized() || objectsNumber < 1) return false;

            for (int i = Capacity - 1; i >= 0; i--)
            {
                StorageZoneColumnCell<TContent> cell = containedObjects[i];
                if (cell.content.IsNull()) continue;
                result = cell.content;
                cell.content = default;
                objectsNumber--;
                InvokeSentCallback();
                return true;
            }

            return false;
            
        }
        
        public override Vector3 GetItemPosition(int index = -1)
        {
            if (!CheckInitialized()) return default;
            
            if (index == -1) index = ObjectsCount;
            bool containsPosition = containedObjectsPositions.ContainsIndex(index);
            if (!containsPosition) RefreshPositionsTo(index);
            return containedObjectsPositions[index];
        }

        #endregion
        
        #region Methods
        
        public void RefreshPattern(GridZonePattern newPattern)
        {
            if (!CheckInitialized()) return;
            RefreshPatternInternal(newPattern);
        }

        private void RefreshPatternInternal(GridZonePattern newPattern)
        {
            this.pattern = newPattern;
            containedObjectsPositions.Clear();
            RefreshPositionsTo(containedObjects.LastIndex());
        }
        
        private void RefreshPositionsTo(int index)
        {
            Vector3 newPosition;
            while (containedObjectsPositions.LastIndex() < index)
            {
                newPosition = pattern.GetPosition(containedObjectsPositions.Count);
                containedObjectsPositions.Add(newPosition);
            }
        }
        
        public bool TrySearchNearestNotFluentlyColumn(out Vector3 result, Vector3 receiverPosition)
        {
            if (receiverPosition == cachedNearestColumnPose && 
                !columnsDictionary[receiverPosition].IsFluently)
            {
                result = receiverPosition;
                return true;
            }

            Vector3 nearestColumnPose = defaultNearestColumnPose;
            float minDistance = defaultMaxDistanceToColumn;
            bool searchingSuccess = false;
            foreach (Vector3 columnPose in columnsDictionary.Keys)
            {
                float distance = Vector3.Distance(columnPose, receiverPosition);
                bool columnIsFluently = columnsDictionary[columnPose].IsFluently;
                if (columnIsFluently || distance > popRadius || distance > minDistance) continue;
                nearestColumnPose = columnPose;
                minDistance = distance;
                searchingSuccess = true;
            }

            result = searchingSuccess ? nearestColumnPose : defaultNearestColumnPose;
            return searchingSuccess;
        }

        #endregion
        
        #region Init Methods

        private void GenerateStructures(int capacity, ref List<StorageZoneColumnCell<TContent>> startedCellsList)
        {
            int columnsNumber = preferences.xRowSize * preferences.zRowSize;
            int cellsNumber = columnsNumber * defaultColumnHeight;
            columnsPositions = new List<Vector3>(columnsNumber);
            columnsDictionary = new Dictionary<Vector3, StorageZoneColumn<TContent>>(columnsNumber);
            containedObjectsPositions = new List<Vector3>(capacity);
            if (startedCellsList.IsNull()) startedCellsList = new List<StorageZoneColumnCell<TContent>>(cellsNumber); 
            containedObjects = startedCellsList;

            FillColumnsDictionaryEmptyCell(columnsNumber);
            GenerateCells(startedCellsList, cellsNumber);
        }

        private void FillColumnsDictionaryEmptyCell(int columnsNumber)
        {
            for (int i = 0; i < columnsNumber; i++)
            {
                StorageZoneColumn<TContent> column = new StorageZoneColumn<TContent>(defaultColumnHeight);
                Vector3 position = pattern.GetPosition(i);
                columnsDictionary.Add(position, column);
                columnsPositions.Add(position);
            }
        }

        private void GenerateCells(List<StorageZoneColumnCell<TContent>> cellsList, int columnsNumber)
        {
            for (int layerIndex = 0; layerIndex < defaultColumnHeight; layerIndex++)
                GenerateLayer(cellsList, layerIndex, columnsNumber);
        }

        private void GenerateLayer(List<StorageZoneColumnCell<TContent>> cellsList, int layerIndex, int columnsNumber)
        {
            int columnIndex = 0;
            foreach (StorageZoneColumn<TContent> column in columnsDictionary.Values)
            {
                GenerateCell(cellsList, column, columnIndex, layerIndex, columnsNumber);
                columnIndex++;
            }
        }
        
        private void GenerateCell(List<StorageZoneColumnCell<TContent>> cellsList,
            StorageZoneColumn<TContent> column, 
            int columnIndex, 
            int layerIndex,
            int columnsNumber)
        {
            int cellIndex = columnIndex + layerIndex * columnsNumber;
            bool needNewCell = !cellsList.ContainsIndex(cellIndex);
            StorageZoneColumnCell<TContent> newCell = needNewCell ? new StorageZoneColumnCell<TContent>() : cellsList[cellIndex];
            
            if (needNewCell) cellsList.Add(newCell);
            column.Add(newCell);
            if (newCell.content.NotNull()) objectsNumber++; 
        }

        #endregion
    }
}
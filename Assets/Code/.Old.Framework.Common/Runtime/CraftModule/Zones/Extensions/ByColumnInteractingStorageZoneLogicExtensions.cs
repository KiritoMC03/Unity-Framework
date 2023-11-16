using System.Collections.Generic;
using General.Extensions;

namespace GameKit.CraftModule.Zones
{
    public static class ByColumnInteractingStorageZoneLogicExtensions
    {
        public static bool TryFindFluentlyCellIndex<T>(this List<StorageZoneColumnCell<T>> list, int capacity, out int index)
        {
            index = -1;
            for (int i = 0; i < capacity; i++)
            {
                if (list[i].content.NotNull()) continue;
                index = i;
                return true;
            }

            return false;
        }
        
        public static bool TryFindFluentlyCellIndex<T>(this List<StorageZoneColumnCell<T>> list, int capacity, out int index, int indexOffset)
        {
            index = -1;
            int currentOffset = 0;
            for (int i = 0; i < capacity; i++)
            {
                if (list[i].content.NotNull()) continue;
                if (currentOffset < indexOffset)
                {
                    currentOffset++;
                    continue;
                }
                index = i;
                return true;
            }

            return false;
        }
    }
}
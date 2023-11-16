using System.Collections.Generic;
using General.Extensions;

namespace GameKit.CraftModule.Zones
{
    public class StorageZoneColumn<TContent>
    {
        #region Fields

        private List<StorageZoneColumnCell<TContent>> value;

        #endregion

        #region Properties

        public List<StorageZoneColumnCell<TContent>> Value => value;

        /// <summary>
        /// O(n)
        /// </summary>
        public bool IsFluently
        {
            get
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i].content.IsNull()) continue;
                    return false;
                }

                return true;
            }
        }

        public int Count => value.Count;

        #endregion

        #region Constructors

        public StorageZoneColumn() => value = new List<StorageZoneColumnCell<TContent>>();

        public StorageZoneColumn(int height) => value = new List<StorageZoneColumnCell<TContent>>(height);

        #endregion

        #region Methods

        public void Add(StorageZoneColumnCell<TContent> item) => value.Add(item);
        public void Add(TContent item) => Add(new StorageZoneColumnCell<TContent>() { content = item });

        public StorageZoneColumnCell<TContent> this[int index]
        {
            get => value[index];
            set => this.value[index] = value;
        }

        public bool TryPopTopObject(out TContent result)
        {
            for (int i = value.Count - 1; i >= 0; i--)
            {
                if (value[i].content.IsNull()) continue;
                result = value[i].content;
                value[i].content = default;
                return true;
            }

            result = default;
            return false;
        }
        
        #endregion
    }
}
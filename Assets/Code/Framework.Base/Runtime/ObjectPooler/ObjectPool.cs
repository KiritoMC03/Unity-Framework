using System.Collections.Generic;
using UnityEngine;

namespace Framework.Base.ObjectPool
{
    public class Pool
    {
        #region Properties

        public ObjectInfo Info { get; }
        public Transform Container { get; }
        public Queue<GameObject> Objects { get; }

        #endregion

        #region Methods

        public Pool(Transform container, ObjectInfo info)
        {
            Info = info;
            Container = container;
            Objects = new Queue<GameObject>(info.startNumber);
        }

        #endregion
    }
}
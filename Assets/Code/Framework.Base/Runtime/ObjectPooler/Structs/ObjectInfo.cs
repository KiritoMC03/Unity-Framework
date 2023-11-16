﻿using System;
using UnityEngine;

namespace ObjectPool
{
    [Serializable]
    public class ObjectInfo
    {
        #region Fields

        public PooledObjectType type;
        public GameObject prefab;
        public int startNumber;
        public bool isDontDestroyOnload;

        #endregion
    }
}
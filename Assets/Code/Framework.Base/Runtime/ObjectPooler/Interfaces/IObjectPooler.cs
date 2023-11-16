using UnityEngine;

namespace ObjectPool
{
    public interface IObjectPooler
    {
        public GameObject GetObject(PooledObjectType type);
        public bool TrySendToPool(GameObject obj);
    }
}
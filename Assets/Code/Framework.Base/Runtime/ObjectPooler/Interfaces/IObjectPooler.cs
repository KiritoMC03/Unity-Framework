using UnityEngine;

namespace Framework.Base.ObjectPool
{
    public interface IObjectPooler
    {
        public GameObject GetObject(PooledObjectType type);
        public bool TrySendToPool(GameObject obj);
    }
}
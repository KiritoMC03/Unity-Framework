using UnityEngine;

namespace Framework.Base.ObjectPool
{
    public class IPooledObject : MonoBehaviour
    {
        public PooledObjectType Type { get; set; }
    }
}
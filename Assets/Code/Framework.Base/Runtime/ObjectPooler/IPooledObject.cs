using UnityEngine;

namespace ObjectPool
{
    public class IPooledObject : MonoBehaviour
    {
        public PooledObjectType Type { get; set; }
    }
}
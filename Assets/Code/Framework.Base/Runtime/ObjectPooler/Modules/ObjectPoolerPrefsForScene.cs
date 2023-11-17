using UnityEngine;

namespace Framework.Base.ObjectPool
{
    public class ObjectPoolerPrefsForScene : MonoBehaviour
    {
        #region Properties

        [field: SerializeField]
        internal PooledObjectType[] RequiredTypes { get; set; }

        #endregion
    }
}
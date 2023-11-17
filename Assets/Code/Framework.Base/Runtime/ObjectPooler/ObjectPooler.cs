using System.Collections.Generic;
using System.IO;
using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Base.ObjectPool
{
    public class ObjectPooler : MonoBehaviour, IObjectPooler
    {
        #region Fields

        [HideInInspector]
        public PooledObjectsInfo pooledObjectsInfo;

        private Dictionary<PooledObjectType, Pool> pools;
        private ObjectPoolerOptimizer optimizer;
        private ObjectPoolerWorkWithScenes workWithScenes;
        private PoolsConstructor constructor;
        private static ObjectPooler instance;
        private bool isInitialized;

        private const string PoolContainsObject = "The pool already contains the target.";
        private const string PooledObjectsInfoPath = "ObjectPooler/PooledObjectsInfoAsset";
        private const string GameObjectName = "ObjectPooler";

        private const string PooledObjectsInfoNotFound =
            "PooledObjectsInfo config not found! Create it from Object Pooler menu, or manually.";

        private static readonly string ObjectHasNoInterfaceFailedReturn =
            $"{nameof(GameObject)} has no interface {nameof(IPooledObject)}. The return to the pool failed.";

        private static readonly string ObjectIsNullFailedReturn =
            $"{nameof(GameObject)} is null. The return to the pool failed.";

        #endregion

        #region Properties

        public static ObjectPooler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ObjectPooler>();
                    if (instance.NotNull() && !instance.isInitialized) instance.InitPool();
                    if (instance.IsNull()) instance = Init();
                }

                return instance;
            }
        }

        #endregion

        #region Unity lifecycle

        private void Start()
        {
            if (isInitialized) return;
            instance = this;
            InitPool();
        }

        private void OnDestroy()
        {
            optimizer?.CheckPoolsSizeNumber();
            workWithScenes?.ClearSubscribes();
        }

        #endregion

        #region IObjectPooler

        public GameObject GetObject(PooledObjectType type)
        {
            if (!pools.ContainsKey(type)) constructor.CreatePool(type);
            GameObject obj = pools[type].Objects.Count > 0
                ? pools[type].Objects.Dequeue()
                : constructor.InstantiateObject(type, pools[type].Container);

            if (obj.IsNull())
                obj = constructor.InstantiateObject(type, pools[type].Container);

            obj.SetActive(true);
            return obj;
        }

        public bool TrySendToPool(GameObject obj)
        {
            if (obj.IsNull())
                Debug.LogWarning(ObjectIsNullFailedReturn);

            if (obj.TryGetComponent(out IPooledObject pooledObject))
            {
                if (!pools[pooledObject.Type].Objects.Contains(obj))
                {
                    pools[pooledObject.Type].Objects.Enqueue(obj);
                    obj.SetActive(false);
                    return true;
                }

                Debug.LogWarning(PoolContainsObject, obj);
                return false;
            }

            Debug.LogWarning(ObjectHasNoInterfaceFailedReturn, obj);
            return false;
        }

        #endregion

        #region Methods

        private static ObjectPooler Init()
        {
            ObjectPooler objectPooler =
                new GameObject(GameObjectName, typeof(ObjectPooler)).GetComponent<ObjectPooler>();
            objectPooler.InitPool();
            return objectPooler;
        }

        private void InitPool()
        {
            pooledObjectsInfo = Resources.Load(PooledObjectsInfoPath) as PooledObjectsInfo;
            if (pooledObjectsInfo.IsNull()) throw new FileNotFoundException(PooledObjectsInfoNotFound);
            pools = new Dictionary<PooledObjectType, Pool>();
            constructor = new PoolsConstructor(transform, pools, pooledObjectsInfo);
            optimizer = new ObjectPoolerOptimizer(pooledObjectsInfo, pools);
            workWithScenes = new ObjectPoolerWorkWithScenes(optimizer, pools, default);

            bool allWithInitAsync =
                pooledObjectsInfo.startedPoolsCreationMode == StartedPoolsCreationMode.AllWithInitAsync;
            if (pooledObjectsInfo.startedPoolsCreationMode == StartedPoolsCreationMode.AllWithInit ||
                allWithInitAsync)
                foreach (ObjectInfo info in pooledObjectsInfo.list)
                    constructor.CreatePool(info, allWithInitAsync);

            DontDestroyOnLoad(gameObject);
            isInitialized = true;
        }

        public void SetGetObjectPoolerPrefsForSceneDelegate(GetObjectPoolerPrefsForSceneDelegate newDelegate) =>
            workWithScenes.getPrefsDelegate = newDelegate;

        #endregion
    }
}
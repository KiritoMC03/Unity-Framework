using System.Collections.Generic;
using General.Extensions;
using UnityEngine.SceneManagement;

namespace ObjectPool
{
    public delegate void GetObjectPoolerPrefsForSceneDelegate(out ObjectPoolerPrefsForScene prefs);

    internal class ObjectPoolerWorkWithScenes
    {
        #region Fields

        internal GetObjectPoolerPrefsForSceneDelegate getPrefsDelegate;
        private readonly ObjectPoolerOptimizer optimizer;
        private readonly Dictionary<PooledObjectType, Pool> pools;

        #endregion

        #region Constructors

        public ObjectPoolerWorkWithScenes(
            ObjectPoolerOptimizer optimizer,
            Dictionary<PooledObjectType, Pool> pools,
            GetObjectPoolerPrefsForSceneDelegate getPrefsDelegate)
        {
            this.optimizer = optimizer;
            this.pools = pools;
            this.getPrefsDelegate = getPrefsDelegate;
            SceneManager.activeSceneChanged += HandleSceneChanged;
        }

        #endregion

        #region Methods

        internal void ClearSubscribes() => SceneManager.activeSceneChanged -= HandleSceneChanged;

        private void HandleSceneChanged(Scene oldScene, Scene newScene)
        {
            ObjectPoolerPrefsForScene prefs = default;
            getPrefsDelegate?.Invoke(out prefs);
            RemoveSceneSpecificPools(prefs);
        }

        private void RemoveSceneSpecificPools(ObjectPoolerPrefsForScene prefs = default)
        {
            optimizer.CheckPoolsSizeNumber();
            pools.RemoveWithSuchValues(pool =>
            {
                bool needDestroy = !pool.Info.isDontDestroyOnload && NotRequireForNewScene(pool.Info.type, prefs);
                if (needDestroy) UnityEngine.Object.Destroy(pool.Container.gameObject);
                return needDestroy;
            });
        }

        private static bool NotRequireForNewScene(PooledObjectType type, ObjectPoolerPrefsForScene prefs = default)
        {
            if (prefs == null) return false;
            PooledObjectType[] requiredTypes = prefs.RequiredTypes;
            int requiredTypesLength = requiredTypes.Length;
            for (int i = 0; i < requiredTypesLength; i++)
                if (requiredTypes[i] == type)
                    return false;
            return true;
        }

        #endregion
    }
}
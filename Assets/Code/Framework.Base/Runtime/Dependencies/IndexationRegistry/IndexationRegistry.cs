using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Base.Dependencies.Indexation
{
    public class IndexationRegistry
    {
        #region Fields

        private static IndexationRegistry instance;

        private readonly Dictionary<string, List<GameObject>> map = new Dictionary<string, List<GameObject>>();

        private readonly Dictionary<string, List<Func<GameObject, bool>>> waitForHandlers =
            new Dictionary<string, List<Func<GameObject, bool>>>();

        #endregion

        #region Properties

        public static IndexationRegistry Instance => instance ??= new IndexationRegistry();

        #endregion

        #region Constructors

        private IndexationRegistry()
        {
        }

        #endregion

        #region Registration

        public void RegisterGameObject<T>(GameObject gameObject, T key)
            where T : IIndexationKey
        {
            string id = key.GetId();
            if (!map.TryGetValue(id, out List<GameObject> gameObjectsList))
                map[id] = gameObjectsList = new List<GameObject>();

            gameObjectsList.Add(gameObject);
            if (!waitForHandlers.TryGetValue(id, out List<Func<GameObject, bool>> handlers)) return;
            for (int i = handlers.Count - 1; i >= 0; i--)
                if (handlers[i].Invoke(gameObject))
                    handlers.RemoveAt(i);
        }

        public void UnregisterGameObject<T>(GameObject gameObject, T key)
            where T : IIndexationKey
        {
            string id = key.GetId();
            if (!map.TryGetValue(id, out List<GameObject> gameObjectsList) || !gameObjectsList.Remove(gameObject))
                return;

            if (gameObjectsList.Count == 0)
                map.Remove(id);
        }

        #endregion

        #region Getters

        public async UniTask<T> GetFirstOrWaitForBehaviour<T, TKey>(TKey key,
            CancellationToken cancellationToken = default)
            where TKey : IIndexationKey
        {
            T result = GetFirstBehaviour<T, TKey>(key);
            if (result.IsNull())
                result = await WaitForAddedBehaviour<T, TKey>(key, cancellationToken);
            return result;
        }

        public async UniTask<GameObject> GetFirstOrWaitForGameObject<TKey>(TKey key,
            CancellationToken cancellationToken = default)
            where TKey : IIndexationKey
        {
            GameObject result = GetFirstGameObject(key);
            if (result.IsNull())
                result = await WaitForAddedGameObject(key, cancellationToken);
            return result;
        }

        public async UniTask<GameObject> WaitForAddedGameObject<TKey>(TKey key,
            CancellationToken cancellationToken = default)
            where TKey : IIndexationKey
        {
            string id = key.GetId();
            bool finished = false;
            GameObject result = default;
            waitForHandlers.TryAdd(id, new List<Func<GameObject, bool>>());
            waitForHandlers[id].Add(TryGet);
            await UniTask.WaitUntil(IsFinished, cancellationToken: cancellationToken);
            return result;

            bool IsFinished() => finished;
            bool TryGet(GameObject added)
            {
                result = added;
                return finished = true;
            }
        }

        public async UniTask<T> WaitForAddedBehaviour<T, TKey>(TKey key,
            CancellationToken cancellationToken = default)
            where TKey : IIndexationKey
        {
            bool finished = false;
            T result = default;
            string id = key.GetId();
            waitForHandlers.TryAdd(id, new List<Func<GameObject, bool>>());
            waitForHandlers[id].Add(TryGet);
            await UniTask.WaitUntil(IsFinished, cancellationToken: cancellationToken);
            return result;

            bool IsFinished() => finished;
            bool TryGet(GameObject added) => finished = added.TryGetComponent(out result);
        }

        public GameObject GetFirstGameObject<TKey>(TKey key)
            where TKey : IIndexationKey =>
            !map.TryGetValue(key.GetId(), out List<GameObject> gameObjects)
                ? default
                : gameObjects.First();

        public T GetFirstBehaviour<T, TKey>(TKey key)
            where TKey : IIndexationKey =>
            !map.TryGetValue(key.GetId(), out List<GameObject> gameObjects)
                ? default
                : gameObjects.First().GetComponent<T>();

        public IEnumerable<GameObject> GetGameObjects<TKey>(TKey key)
            where TKey : IIndexationKey =>
            !map.TryGetValue(key.GetId(), out List<GameObject> gameObjects)
                ? Enumerable.Empty<GameObject>()
                : gameObjects;

        public IEnumerable<T> GetBehaviours<T, TKey>(TKey key)
            where TKey : IIndexationKey =>
            !map.TryGetValue(key.GetId(), out List<GameObject> gameObjects)
                ? Enumerable.Empty<T>()
                : gameObjects.SelectMany(gameObject => gameObject.GetComponents<T>());

        #endregion
    }
}
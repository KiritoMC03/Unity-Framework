using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Base.Dependencies.Indexation
{
    [Serializable]
    public abstract class BaseIndexReference<TKey>
        where TKey : class, IIndexationKey
    {
        public abstract IEnumerable<TKey> IterateIndexes();

        public IEnumerable<GameObject> IterateGameObjects()
        {
            foreach (TKey index in IterateIndexes())
            foreach (GameObject gameObject in IndexationRegistry.Instance.GetGameObjects(index))
                yield return gameObject;
        }

        public IEnumerable<T> IterateBehaviours<T>()
        {
            foreach (TKey index in IterateIndexes())
            foreach (T behaviour in IndexationRegistry.Instance.GetBehaviours<T, TKey>(index))
                yield return behaviour;
        }

        public GameObject FirstGameObject() => First(IndexationRegistry.Instance.GetFirstGameObject);
        public T FirstBehaviour<T>() => First(IndexationRegistry.Instance.GetFirstBehaviour<T, TKey>);

        public async UniTask<GameObject> WaitForAddedGameObject() =>
            (await UniTask.WhenAny(GetTasks(IndexationRegistry.Instance.WaitForAddedGameObject)))
            .result;

        public async UniTask<T> WaitForAddedBehaviour<T>() =>
            (await UniTask.WhenAny(GetTasks(IndexationRegistry.Instance.WaitForAddedBehaviour<T, TKey>)))
            .result;

        public async UniTask<GameObject> FirstOrWaitForGameObject() =>
            (await UniTask.WhenAny(GetTasks(IndexationRegistry.Instance.GetFirstOrWaitForGameObject)))
            .result;

        public async UniTask<T> FirstOrWaitForBehaviour<T>() =>
            (await UniTask.WhenAny(GetTasks(IndexationRegistry.Instance.GetFirstOrWaitForBehaviour<T, TKey>)))
            .result;

        private IEnumerable<UniTask<T>> GetTasks<T>(
            Func<TKey, CancellationToken, UniTask<T>> func,
            CancellationToken cancellationToken = default)
        {
            foreach (TKey index in IterateIndexes())
                yield return func(index, cancellationToken);
        }

        private T First<T>(Func<TKey, T> func)
        {
            foreach (TKey index in IterateIndexes())
            {
                T result = func(index);
                if (result.NotNull())
                    return result;
            }

            return default;
        }
    }
}
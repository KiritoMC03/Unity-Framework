using General;
using ObjectPool;
using UnityEngine;

namespace GameKit.CraftModule.ResourceCreator
{
    /// <summary>
    /// Where TInput is Serializable type.
    /// </summary>
    /// <typeparam name="TInput">Serializable type.</typeparam>
    public abstract class ToPoolerMatcherConfig<TInput> : ScriptableObject
    {
        public SerializedDictionary<TInput, PooledObjectType> typesMatching;
    }
}
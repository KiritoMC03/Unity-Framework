using Framework.Base.Collections;
using Framework.Base.ObjectPool;

namespace Framework.Idlers.ResourceCreator
{
    /// <summary>
    /// Where TInput is Serializable type.
    /// </summary>
    /// <typeparam name="TInput">Serializable type.</typeparam>
    public abstract class ToPoolerMatcherConfig<TInput> : UnityEngine.ScriptableObject
    {
        public SerializedDictionary<TInput, PooledObjectType> typesMatching;
    }
}
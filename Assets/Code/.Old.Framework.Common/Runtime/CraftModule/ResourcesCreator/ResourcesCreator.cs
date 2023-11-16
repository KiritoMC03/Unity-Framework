using General;
using General.Mediator;
using ObjectPool;
using UnityEngine;

namespace GameKit.CraftModule.ResourceCreator
{
    public class ResourcesCreator<TComponent, TMatcher> : ISingleComponent
    {
        #region Fields

        private SerializedDictionary<TMatcher, PooledObjectType> typesMatching;

        private ObjectPooler pooler;

        private readonly string ObjectDoesntContainsComponent = $"Object doesn`t contains Interface for {typeof(TComponent)}.";
        
        #endregion

        #region Constructors

        public ResourcesCreator(SerializedDictionary<TMatcher, PooledObjectType> typesMatching)
        {
            this.typesMatching = typesMatching;
            pooler = ObjectPooler.Instance;
        }

        #endregion

        #region Methods

        public TComponent GetResource(TMatcher type)
        {
            if (!typesMatching.ContainsKey(type))
            {
                Debug.LogError($"{nameof(typesMatching)} not contains definition for {type}");
                return default;
            }

            return pooler.GetObject(typesMatching[type]).GetComponent<TComponent>();
        }

        public bool TryGetResource(TMatcher type, out TComponent resource)
        {
            if (!typesMatching.ContainsKey(type))
            {
                Debug.LogWarning($"{nameof(typesMatching)} not contains definition for {type}");
                resource = default;
                return false;
            }
            
            var isSuccess = pooler.GetObject(typesMatching[type]).TryGetComponent(out resource);
            if (isSuccess) return true;

            Debug.LogWarning(ObjectDoesntContainsComponent);
            return false;
        }

        public void DestroyResource(GameObject target)
        {
            pooler.TrySendToPool(target);
        }

        #endregion
    }
}
using UnityEngine;

namespace Framework.Idlers.CollisionResolver
{
    public class CollisionResolver : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private float lifeTime = 5f;

        [SerializeField]
        private DestroyMode destroyMode = DestroyMode.Component;

        [SerializeField] [Range(0, 1)]
        private float offsetSpeed;
        
        private Transform cacheTransform;

        #endregion
        
        #region Properties

        public Transform CacheTransform
        {
            get
            {
                if (cacheTransform == null)
                {
                    cacheTransform = transform;
                }

                return cacheTransform;
            }
        }

        #endregion

        #region Unity lifecycle
        
        private void OnEnable()
        {
            switch (destroyMode)
            {
                case DestroyMode.Component:
                    Destroy(this, lifeTime);
                    break;
                case DestroyMode.GameObject: 
                    Destroy(gameObject, lifeTime);
                    break;
                default:
                    Destroy(this, lifeTime);
                    break;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out ICollisionResolverTarget target))
            {
                var direction = target.Transform.position - CacheTransform.position;
                direction.y = 0;
                target.Transform.position += direction * offsetSpeed;
            }
        }

        #endregion
    }
}
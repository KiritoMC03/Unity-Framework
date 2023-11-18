using UnityEngine;

namespace Framework.Idlers.CollisionResolver
{
    public class TransparentCollisionResolver : MonoBehaviour
    {
        #region Properties
        
        [field: SerializeField]
        public Collider Collider { get; protected set; }

        #endregion

        #region Unity lifecycle

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ICollisionResolverTarget target)) 
                Collider.isTrigger = false;
        }

        #endregion

        #region Methods

        public virtual void MakeRigid() => Collider.isTrigger = false;
        public virtual void MakeTransparent() => Collider.isTrigger = true;
        public virtual void Destroy() => Destroy(this);

        #endregion
    }
}
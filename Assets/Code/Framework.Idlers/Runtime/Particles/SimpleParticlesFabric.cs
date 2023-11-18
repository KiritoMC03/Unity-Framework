using Framework.Base.ObjectPool;
using UnityEngine;

namespace Framework.Idlers.Particles
{
    public class SimpleParticlesFabric
    {
        #region Methods
        
        public virtual void ShowTemp(SimpleParticle particle, Vector3 position)
        {
            particle.transform.position = position;
            particle.Show().Play().InvokeOnComplete(Destroy);
        }

        public virtual void Destroy(SimpleParticle particle) => 
            ObjectPooler.Instance.TrySendToPool(particle.gameObject);

        #endregion
    }
}
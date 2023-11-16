using ObjectPool;
using UnityEngine;

namespace GameKit.General.Particles
{
    public class GKParticlesFabric
    {
        #region Methods
        
        public void ShowTemp(SimpleParticle particle, Vector3 position)
        {
            particle.transform.position = position;
            particle.Show().Play().InvokeOnComplete(Destroy);
        }

        public void Destroy(SimpleParticle particle) => 
            ObjectPooler.Instance.TrySendToPool(particle.gameObject);

        #endregion
    }
}
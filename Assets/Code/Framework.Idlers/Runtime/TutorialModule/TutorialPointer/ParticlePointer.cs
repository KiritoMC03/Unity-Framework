using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Idlers.TutorialModule
{
    public class ParticlePointer : MonoBehaviour, ITutorialPointer
    {
        #region Fields

        [SerializeField]
        protected ParticleSystem system;

        #endregion

        #region Properties
        
        public virtual GameObject GameObject => gameObject;

        #endregion

        #region ITutorialPointer
        
        public virtual void StartHesitation(Vector3 position)
        {
            Show();
            transform.position = position;
            system.Play();
        }

        public virtual void StopHesitation()
        {
            system.Stop();
        }

        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
        public virtual void Destroy() => gameObject.DestroyNotNull();

        #endregion
    }
}
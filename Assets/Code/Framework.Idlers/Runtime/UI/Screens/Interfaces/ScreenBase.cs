using UnityEngine;

namespace Framework.Idlers.UI
{
    public abstract class ScreenBase : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public virtual bool ReuseInstance { get; protected set; }

        #endregion

        #region Methods

        public abstract void Init();

        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);

        #endregion
    }
}
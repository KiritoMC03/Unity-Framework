using System;
using UnityEngine;

namespace Framework.Base.Physics
{
    public class TriggerListener : MonoBehaviour
    {
        #region Events

        public event Action<Collider> TriggerEnterCallback;
        public event Action<Collider> TriggerExitCallback;

        #endregion

        #region Unity lifecycle

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnterCallback?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExitCallback?.Invoke(other);
        }

        #endregion
    }
}
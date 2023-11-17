using System;
using UnityEngine;

namespace Framework.Base.Physics
{
    public class CollisionListener : MonoBehaviour
    {
        #region Events

        public event Action<Collision> CollisionEnterCallback;
        public event Action<Collision> CollisionExitCallback;

        #endregion

        #region Unity lifecycle

        private void OnCollisionEnter(Collision other)
        {
            CollisionEnterCallback?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            CollisionExitCallback?.Invoke(other);
        }

        #endregion
    }
}
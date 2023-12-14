using System;
using System.Threading.Tasks;
using Framework.Base.ComponentModel;
using Framework.Base.Extensions;
using UnityEngine;
using Component = Framework.Base.ComponentModel.Component;

namespace Code
{
    [Serializable] [AutoContainer]
    public class Player : Component
    {
        public Rigidbody rigidbody;

        protected override async void Construct()
        {
            new Enemy().GetComponent<ЧтоТО>();
            while (source.NotNull())
            {
                rigidbody.AddForce(Vector3.forward);
                await Task.Yield();
            }
        }
    }

    [Serializable] [AutoContainer]
    public class Enemy : Component
    {
        [SerializeField]
        private Rigidbody rigidbody;
        
        public Vector3 GetPosition() => source.transform.position;
        public void AddForce(Vector3 force) => rigidbody.AddForce(force);
    }
}
using UnityEngine;

namespace Framework.Base.ComponentModel
{
    public class ComponentContainer<T> : MonoBehaviour
        where T: Component, new()
    {
        [field: SerializeField]
        public T Component { get; private set; }

        /// <summary>
        /// Awake used to create connection between ContainerT and T-component.
        /// The basic idea - all containers in the scene will be initialized in 1 pass and then be ready-for-use.
        /// </summary>
        protected virtual void Awake() => Component.source = this;
        protected virtual void Start() => Component.Construct();

        public virtual void Set(T component) => Component = component;
        public virtual void SetAndAwake(T component)
        {
            Component = component;
            Awake();
        }
    }
}
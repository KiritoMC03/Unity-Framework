using Framework.Base;
using UnityEngine;

namespace Framework.Idlers.ScriptableObject
{
    [CreateAssetMenu(fileName = "UniqueScriptableObjectIdentifier", menuName = "Framework/Idlers/UniqueScriptableObjectIdentifier", order = 0)]
    public sealed class UniqueScriptableObjectIdentifier : UnityEngine.ScriptableObject
    {
        [SerializeField] [ReadOnly]
        internal bool isValid = true;

        internal void OnValidate()
        {
            if (!isValid)
                Debug.LogError($"Invalid {nameof(UniqueScriptableObjectIdentifier)} found! " +
                    $"Another {nameof(UniqueScriptableObjectIdentifier)} with this name ({this.name}) already exist", this);
        }

        public override bool Equals(object other)
        {
            if (other is UniqueScriptableObjectIdentifier i)
                return Equals(i);
            return base.Equals(other);
        }

        public bool Equals(UniqueScriptableObjectIdentifier other) => this.name == other.name;
        public override int GetHashCode() => this.name.GetHashCode();
    }
}
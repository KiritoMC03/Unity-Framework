using System;
using UnityEngine;

namespace GameKit.General.Structures
{
    [Serializable]
    public abstract class StringBasedIdentifier : IEquatable<StringBasedIdentifier>
    {
        #region Fields

        [SerializeField]
        protected internal string value;

        private readonly string toString;

        #endregion

        #region Properties

        public string Value => value;

        #endregion

        #region Constructors

        public StringBasedIdentifier(string value)
        {
            this.value = value;
            toString = $"{GetType().Name}.{value}";
        }

        #endregion

        #region IEquatable<ResourceType>

        public bool Equals(StringBasedIdentifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return value == other.value;
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return (StringBasedIdentifier)other == this;
        }

        public override int GetHashCode() => value != null ? value.GetHashCode() : 0;

        #endregion

        #region Methods

        public override string ToString() => toString;

        #endregion

        #region Operators

        public static bool operator ==(StringBasedIdentifier type1, StringBasedIdentifier type2) =>
            type1.value == type2.value;

        public static bool operator !=(StringBasedIdentifier type1, StringBasedIdentifier type2) =>
            type1.value != type2.value;

        #endregion
    }
}
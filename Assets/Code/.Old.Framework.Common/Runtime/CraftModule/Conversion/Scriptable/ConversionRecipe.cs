using UnityEngine;

namespace GameKit.Conversion
{
    public abstract class ConversionRecipe<T> : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private T input;

        [SerializeField]
        private T output;

        #endregion

        #region Properties

        public T Input => input;
        public T Output => output;

        #endregion
    }
}
using System;
using Random = UnityEngine.Random;

namespace GameKit.General.Structures
{
    [Serializable]
    public struct FloatRange
    {
        #region Fields

        public float min;
        public float max;

        #endregion

        #region Methods

        public float GetRandom() => Random.Range(min, max);

        #endregion
    }
}
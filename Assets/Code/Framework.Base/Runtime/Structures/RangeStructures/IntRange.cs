using System;
using Random = UnityEngine.Random;

namespace GameKit.General.Structures
{
    [Serializable]
    public struct IntRange
    {
        #region Fields

        public int min;
        public int max;

        #endregion

        #region Methods

        public int GetRandom() => Random.Range(min, max + 1);

        #endregion
    }
}
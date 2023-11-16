using System;

namespace General.Editor
{
    public class DataType
    {
        #region Fields

        private readonly Type type;
        private readonly string name;

        #endregion

        #region Fields

        public Type Type => type;
        public string Name => name;

        #endregion

        #region Class lifecycle

        public DataType(Type type, string name)
        {
            this.type = type;
            this.name = name;
        }

        #endregion
    }
}
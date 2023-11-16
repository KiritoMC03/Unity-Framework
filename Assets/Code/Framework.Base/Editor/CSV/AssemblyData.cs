using System.Collections.Generic;

namespace General.Editor
{
    public class AssemblyData
    {
        #region Fields

        private readonly List<DataType> structureDataTypes;
        private readonly List<string> wrapperDataNames;

        #endregion

        #region Properties

        public List<DataType> StructureDataTypes => structureDataTypes;
        public List<string> WrapperDataNames => wrapperDataNames;

        #endregion

        #region Class lifecycle

        public AssemblyData(List<DataType> structureDataTypes, List<string> wrapperDataNames)
        {
            this.structureDataTypes = structureDataTypes;
            this.wrapperDataNames = wrapperDataNames;
        }

        #endregion
    }
}
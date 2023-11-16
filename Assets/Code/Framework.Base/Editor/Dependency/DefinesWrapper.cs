using System.Collections.Generic;

namespace General.Editor
{
    [Data("DC.json")]
    public struct DefinesWrapper
    {
        public List<Define> Defines;

        public DefinesWrapper(List<Define> defines = null) => Defines = defines ?? new List<Define>();
    }
}
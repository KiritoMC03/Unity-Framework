using System.Collections.Generic;
using Framework.Base.SaveLoad;

namespace Framework.Base.Editor
{
    [Data("DC.json")]
    public struct DefinesWrapper
    {
        public List<Define> Defines;

        public DefinesWrapper(List<Define> defines = null) => Defines = defines ?? new List<Define>();
    }
}
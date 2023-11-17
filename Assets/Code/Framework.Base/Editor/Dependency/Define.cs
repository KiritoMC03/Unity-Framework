using System;
using System.Collections.Generic;

namespace Framework.Base.Editor
{
    [Serializable]
    public class Define
    {
        public List<string> defines;
        public string sectionName;

        public Define(List<string> defines, string sectionName)
        {
            this.defines = defines;
            this.sectionName = sectionName;
        }
    }
}
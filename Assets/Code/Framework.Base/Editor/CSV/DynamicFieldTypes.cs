using System.Collections.Generic;
using System.Reflection.Emit;

namespace Framework.Base.Editor
{
    public class DynamicFieldTypes
    {
        private Dictionary<string, FieldBuilder> fieldBuilders = new Dictionary<string, FieldBuilder>();
        public Dictionary<string, FieldBuilder> FieldBuilders => fieldBuilders;
    }
}
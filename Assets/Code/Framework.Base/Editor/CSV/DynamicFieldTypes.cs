using System.Collections.Generic;
using System.Reflection.Emit;

namespace General.Editor
{
    public class DynamicFieldTypes
    {
        private Dictionary<string, FieldBuilder> fieldBuilders = new Dictionary<string, FieldBuilder>();
        public Dictionary<string, FieldBuilder> FieldBuilders => fieldBuilders;
    }
}
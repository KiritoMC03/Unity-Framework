using System;

namespace GameKit.General.Utils
{
    public class ScriptDescription
    {
        public readonly Type type;
        public readonly string path;

        public ScriptDescription(Type type, string path)
        {
            this.type = type;
            this.path = path;
        }
    }
}
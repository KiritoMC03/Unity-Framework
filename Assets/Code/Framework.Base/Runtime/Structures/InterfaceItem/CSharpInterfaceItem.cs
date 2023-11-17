using System;
using Framework.Base.Extensions;

namespace Framework.Base
{
    /// <summary>
    ///  For select non MonoBehaviour interface implementation.
    /// </summary>
    /// <remarks>Used reflection! But only once in runtime.</remarks>
    [Serializable]
    public class CSharpInterfaceItem<T>
    {
        public string typeFullName;
        public string assembly;
        private T item;

        /// <summary>
        ///  Return selected in inspector type.
        /// </summary>
        /// <remarks>Used reflection! But only once in runtime.</remarks>
        public T Get()
        {
            if (item.IsNull()) GetNew();
            return item;
        }

        /// <summary>
        ///  Create and return selected in inspector type.
        /// </summary>
        /// <remarks>Used reflection!</remarks>
        public T GetNew()
        {
            Type type = Type.GetType($"{typeFullName}, {assembly}");
            if (type == default) return default;
            object result = Activator.CreateInstance(type);
            item = (T)result;
            return item;
        }
    }
}
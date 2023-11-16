#if UNITY_EDITOR && NET_4_6
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace General.Editor
{
    public class DynamicBuilderType
    {
        #region Fields

        private const string NameAlreadyExists = "Name already exists.";
        private DynamicFieldTypes dynamicFieldTypes;
        private TypeBuilder typeBuilder;
        private bool isCreate = false;
        private Type type;

        #endregion

        #region Class lifecycle

        public DynamicBuilderType(TypeBuilder typeBuilder)
        {
            dynamicFieldTypes = new DynamicFieldTypes();
            this.typeBuilder = typeBuilder;
        }

        #endregion

        #region Methods

        public DynamicBuilderType CreateField(Type fieldType, string fieldName = "DynamicField")
        {
            if (dynamicFieldTypes.FieldBuilders.ContainsKey(fieldName)) throw new AggregateException(NameAlreadyExists);
            FieldBuilder fieldBuilder = typeBuilder.DefineField(fieldName,
                fieldType, FieldAttributes.Public);
            dynamicFieldTypes.FieldBuilders.Add(fieldName, fieldBuilder);
            return this;
        }

        public Type CreateType()
        {
            if (!isCreate)
            {
                isCreate = true;
                type = typeBuilder.CreateType();
            }

            return type;
        }

        #endregion
    }
}
#endif
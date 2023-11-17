#if UNITY_EDITOR && NET_4_6
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEditor;


namespace Framework.Base.Editor
{
    public class DynamicBuilder
    {
        #region Fields

        public const string ExtensionDLL = ".dll";
        private const string AssemblybuilderIsNull = "AssemblyBuilder is null.";
        private const string NameAlreadyExists = "Name already exists.";
        public readonly string DynamicAssemblyDll;
        private readonly AssemblyName assemblyName;
        private readonly AssemblyBuilder assemblyBuilder;
        private readonly ModuleBuilder moduleBuilder;
        private Dictionary<string, DynamicBuilderType> dynamicTypes = new Dictionary<string, DynamicBuilderType>();

        #endregion

        #region Class lifecycle

        public DynamicBuilder(string assemblyName = "DynamicAssembly")
        {
            this.assemblyName = new AssemblyName();
            this.assemblyName.Name = assemblyName;
            DynamicAssemblyDll = assemblyName + ExtensionDLL;
            assemblyBuilder =
                AssemblyBuilder.DefineDynamicAssembly(this.assemblyName, AssemblyBuilderAccess.RunAndSave);
            moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule", DynamicAssemblyDll);
        }

        #endregion

        #region Methods

        public DynamicBuilderType CreateType(string typeName, Type parent = null)
        {
            if (assemblyBuilder is null) throw new NullReferenceException(AssemblybuilderIsNull);
            if (dynamicTypes.ContainsKey(typeName)) throw new AggregateException(NameAlreadyExists);
            TypeBuilder typeBuilder;
            if (parent is null)
                typeBuilder = moduleBuilder.DefineType(assemblyName.Name + "." + typeName,
                    TypeAttributes.Serializable | TypeAttributes.Class | TypeAttributes.Public);
            else
                typeBuilder = moduleBuilder.DefineType(assemblyName.Name + "." + typeName,
                    TypeAttributes.Serializable | TypeAttributes.Class | TypeAttributes.Public, parent);

            DynamicBuilderType dynamicBuilderType = new DynamicBuilderType(typeBuilder);
            dynamicTypes.Add(typeName, dynamicBuilderType);
            return dynamicBuilderType;
        }


        public void Save()
        {
            foreach (KeyValuePair<string, DynamicBuilderType> item in dynamicTypes) item.Value.CreateType();

            assemblyBuilder.Save(DynamicAssemblyDll);
        }

        #endregion
    }
}

#endif
using System;
using System.Collections.Generic;
using General.Extensions;
using General.Mediator;
using UnityEngine;

namespace GameKit.General.LocalDB
{
    [Serializable]
    public class ConfigsDB : ISingleComponent
    {
        #region Fields

        [SerializeField]
        protected UnityEngine.ScriptableObject[] configs = Array.Empty<UnityEngine.ScriptableObject>();
        
        protected Dictionary<Type, UnityEngine.ScriptableObject> configsDictionary = new Dictionary<Type, UnityEngine.ScriptableObject>(10);
        protected UnityEngine.ScriptableObject tempScriptableObject;
        
        #endregion

        #region Constructors

        public ConfigsDB() => MC.Instance.Add(this, SetMode.Force);

        #endregion

        #region Methods

        public virtual T Get<T>()
            where T : UnityEngine.ScriptableObject
        {
            Type type = typeof(T);
            if (configsDictionary.TryGetValue(type, out tempScriptableObject) && 
                tempScriptableObject.NotNull())
                return (T) tempScriptableObject;

            T result = FindConfig<T>();
            if (result.IsNull()) return result;
            configsDictionary.AddOrReplace(type, result);
            return result;
        }

        public virtual bool TryGet<T>(out T config)
            where T : UnityEngine.ScriptableObject
        {
            config = Get<T>();
            return config.NotNull();
        }

        protected virtual T FindConfig<T>()
            where T : UnityEngine.ScriptableObject
        {
            for (int i = 0; i < configs.Length; i++)
            {
                tempScriptableObject = configs[i];
                if (tempScriptableObject is T config)
                    return config;
            }

            return default;
        }

        #endregion
    }
}
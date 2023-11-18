using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Base.Dependencies.Mediator
{
    internal class BaseComponentList : ArrayList
    {
        #region Fields

        private const string ValueIsNull = "Value is null.";
        protected Dictionary<Type, ComponentData> ComponentsList = new Dictionary<Type, ComponentData>();

        #endregion


        #region Properties

        /// <summary>
        /// Searching for an object in ArrayList via type
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <param name="type"></param>
        /// <returns></returns>
        public object this[Type type]
        {
            get
            {
                if (ComponentsList.TryGetValue(type, out ComponentData data))
                {
                    return this[data.Index];
                }

                return null; // TODO : Mediator - the future needs to be changed to NullObject
            }
        }

        #endregion


        #region Methods
        

        /// <summary>
        /// Adds an object to the end of the ArrayList.
        /// </summary>
        /// <remarks> O(1) | O(N) </remarks>
        /// <param name="value"></param>
        /// <param name="setMode"></param>
        /// <returns></returns>
        public virtual int Add<T>(T value, SetMode setMode, ComponentType componentType) where T : class
        {
            switch (setMode)
            {
                case SetMode.None:
                    return Add(value);

                case SetMode.Force:
                    var type = typeof(T);
                    if (!ComponentsList.ContainsKey(type))
                    {
                        int index = Add(value);
                        ComponentsList.Add(type, new ComponentData(index, true, componentType));
                        return index;
                    }
                    else
                    {
                        ComponentsList.TryGetValue(type, out ComponentData data);
                        data = new ComponentData(data.Index, true, componentType);
                        ComponentsList[type] = data;
                        return Add(value, data.Index);
                    }

                default:
                    return default;
            }
        }

        #region NullObject
        
        /// <summary>
        /// NullObject
        /// </summary>
        /// <param name="obj"></param>
        public override void Remove(object obj){}

        /// <summary>
        /// NullObject
        /// </summary>
        /// <param name="index"></param>
        public override void RemoveAt(int index){}

        /// <summary>
        /// NullObject
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public override void RemoveRange(int index, int count){}
        
        #endregion

        protected BaseComponentList(in int index) : base(index)
        {
        }
        
        /// <summary>
        /// Adds an object to the end of the ArrayList.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected int Add<T>(T value, int index)
        {
            if (value == null)
            {
                Debug.LogWarning(ValueIsNull);
                return default;
            }

            this[index] = value;
            return index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        protected void RemoveType(in Type type,ref object obj, ref ComponentData data)
        {
            Remove(data.Index);
            obj = null;
            data.IsExist = false;
            ComponentsList[type] = data;
        }

        /// <summary>
        /// Removes the object from the ArrayList.
        /// </summary>
        /// <remarks> O(N) </remarks>
        /// <param name="type"></param>
        protected void Remove(Type type)
        {
            base.Remove(this[type]);
        }

        /// <summary>
        /// Removes the object from the ArrayList.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <param name="index"></param>
        protected void Remove(int index)
        {
            this[index] = null;
        }
        
        #endregion
    }
}
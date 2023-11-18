using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Base.Extensions;
#if MEDIATOR_STAKE_TRACE
using Framework.Base.Dependencies.Mediator.Runtime;
#endif
using UnityEngine;

namespace Framework.Base.Dependencies.Mediator
{
    public sealed class MediatorSystem : IMediatorObserver
    {

        #region Fields
        
        private const string TypeIsNull = "Type is null.";
        private const string ValueIsNull = "Value is null.";
        private const string PermissionTypesIsNull = "PermissionTypes is null.";
        private const string TimeMessage = "Search time is zero or less than zero.";
        private const string ArgumentOutOfRangeException = "ArgumentOutOfRangeException";

        private readonly Type iComponents;
        private readonly Type iSingleComponent;
        private readonly Type iWeakSingleComponent;

        private readonly SingleComponentList singleComponentList = new SingleComponentList(30);
        private readonly WeakSingleComponentList weakSingleComponentList = new WeakSingleComponentList(20);
        private readonly ComponentList componentList = new ComponentList(25);
        private readonly PermissionChecker permissionChecker = new PermissionChecker();
        private readonly Dictionary<Type, MediatorInterfaces> typeOfInterfaces = new Dictionary<Type, MediatorInterfaces>();
        private readonly IObserversSystem observersSystem;

        #endregion


        #region Properties
        
        internal Dictionary<Type, MediatorInterfaces> TypeOfInterfaces => typeOfInterfaces;
        internal IObserversSystem ObserversSystem => observersSystem;
        
        #endregion


        #region Methods

        internal MediatorSystem()
        {
            iComponents = typeof(IComponents);
            iSingleComponent = typeof(ISingleComponent);
            iWeakSingleComponent = typeof(IWeakSingleComponent);
            observersSystem = new ObserversSystem();
        }

        /// <summary>
        /// Adds an item by type.
        /// </summary>
        /// <remarks>  O(1) | O(N) </remarks>
        /// <typeparam name="T">class or struct</typeparam>
        /// <param name="value"></param>
        public int Add<T>(in T value, in SetMode setMode = default) where T : class
        {
#if MEDIATOR_STAKE_TRACE
            MediatorStackTrace.GetStackTrace("Add");
#endif
            
            if (value.LogIfNull(ValueIsNull)) return -1;
            
            return AddInternal<T>(value,null,null,setMode);
        }

        /// <summary>
        /// Adds item and permissions by type.
        /// </summary>
        /// <remarks> O(1) | O(N) </remarks>
        /// <typeparam name="T">class or struct</typeparam>
        /// <param name="value"></param>
        /// <param name="permissionTypes"></param>
        public int Add<T>(in T value, in SetMode setMode = default, params Type[] permissionTypes) where T : class
        {
#if MEDIATOR_STAKE_TRACE
            MediatorStackTrace.GetStackTrace("Add");
#endif
            if (value.LogIfNull(ValueIsNull)) return -1;
            
            if (permissionTypes.LogIfNull(PermissionTypesIsNull)) return -1;

            return AddInternal(value, permissionTypes, permissionChecker.SetPermission,setMode);
        }

        /// <summary>
        /// Set lock state for type.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <param name="type"></param>
        /// <param name="lockedState"></param>
        public void SetLock(in Type type,in bool lockedState)
        {
#if MEDIATOR_STAKE_TRACE
            MediatorStackTrace.GetStackTrace("SetLock");
#endif
            if (type.LogIfNull(TypeIsNull)) return;
            
            permissionChecker.SetLock(type,lockedState);
        }

        /// <summary>
        /// Returns an items by type.
        /// </summary>
        /// <remarks> O(N) </remarks>
        /// <typeparam name="T">class or struct</typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="appealType"></param>
        /// <param name="item"></param>
        /// <returns>True if the contains elements with the specified type, otherwise, False.</returns>
        public bool GetComponents<T,U>(T appealType, out List<U> item, bool loging = true)
        {
#if MEDIATOR_STAKE_TRACE
            MediatorStackTrace.GetStackTrace("GetComponents");
#endif
            var type = appealType.GetType();
            var type1 = typeof(U);
            var status = false;
            item = null;
            if (!permissionChecker.IsLocked(type1))
            {
                switch (permissionChecker.CheckPermission(type, type1))
                {
                    case PermissionStates.HaveAccess:
                        status = componentList.TryGet(out item);
                        break;
                    case PermissionStates.HaveNoAccess:
                        Debug.LogWarning($"Have No Access {type} -> {type1}");
                        break;
                    case PermissionStates.PermissionNotFound:
                        status = componentList.TryGet(out item);
                        break;
                    default:
                        Debug.LogError(ArgumentOutOfRangeException);
                        break;
                }
            }
            if(loging) NotFoundComponent(item);
            return status;
        }

        /// <summary>
        /// Returns an item by type.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <typeparam name="U">class or struct</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="appealType"></param>
        /// <param name="item"></param>
        /// <returns>True if the contains an element with the specified type, otherwise, False.</returns>
        public bool GetSingleComponent<T,U>(T appealType, out U item, bool loging = true)
        {
#if MEDIATOR_STAKE_TRACE
            MediatorStackTrace.GetStackTrace("GetSingleComponent");
#endif
            var type = appealType.GetType();
            var type1 = typeof(U);
            var status = false;
            item = default;
            if (!permissionChecker.IsLocked(type1))
            {
                switch (permissionChecker.CheckPermission(type, typeof(U)))
                {
                    case PermissionStates.HaveAccess:
                        status = singleComponentList.TryGet(type1, out object obj);
                        item = (U) obj;
                        break;
                    case PermissionStates.HaveNoAccess:
                        Debug.LogWarning($"Have No Access {type} -> {type1}");
                        break;
                    case PermissionStates.PermissionNotFound:
                        status = singleComponentList.TryGet(type1, out obj);
                        item = (U) obj;
                        break;
                    default:
                        Debug.LogError(ArgumentOutOfRangeException);
                        break;
                }
            }
            if(loging) NotFoundComponent(item);
            return status;
        }

        /// <summary>
        /// Returns an item by type.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <typeparam name="U">class or struct</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="appealType"></param>
        /// <param name="item"></param>
        /// <returns>True if the contains an element with the specified type, otherwise, False.</returns>
        public bool GetWeakSingleComponent<T,U>(T appealType, out U item, bool loging = true) where U : class
        {
#if MEDIATOR_STAKE_TRACE
            MediatorStackTrace.GetStackTrace("GetWeakSingleComponent");
#endif
            var type = appealType.GetType();
            var type1 = typeof(U);
            var status = false;
            item = default;
            if (!permissionChecker.IsLocked(type1))
            {
                WeakReference<U> weak;
                switch (permissionChecker.CheckPermission(type, typeof(U)))
                {
                    case PermissionStates.HaveAccess:
                        status = weakSingleComponentList.TryGet(type1, out object obj);
                        weak = (WeakReference<U>) obj;
                        weak.TryGetTarget(out item);
                        break;
                    case PermissionStates.HaveNoAccess:
                        Debug.LogWarning($"Have No Access {type} -> {type1}");
                        break;
                    case PermissionStates.PermissionNotFound:
                        status = weakSingleComponentList.TryGet(type1, out obj);
                        weak = (WeakReference<U>) obj;
                        weak.TryGetTarget(out item);
                        break;
                    default:
                        Debug.LogError(ArgumentOutOfRangeException);
                        break;
                }
            }
            if(loging) NotFoundComponent(item);
            return status;
        }

        /// <summary>
        /// Searches for a list of items by type for a limited time, if it finds it returns.
        /// </summary>
        /// <remarks> O(N) </remarks>
        /// <param name="appealType"></param>
        /// <param name="items"></param>
        /// <param name="searchTime"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns>True if the contains an element with the specified type, otherwise, False.</returns>
        public async Task<bool> GetComponentsAsync<T,U>(T appealType, Action<List<U>> items,float searchTime = 5f)
        {
#if MEDIATOR_STAKE_TRACE
            MediatorStackTrace.GetStackTrace("GetComponentsAcync");
#endif
            float time = 0;
            if (searchTime <= 0)
            {
                Debug.LogWarning(TimeMessage);
                return false;
            }
            while (time <= searchTime)
            {
                time += Time.deltaTime;
                if (GetComponents(appealType, out List<U> list,false) && list.Count > 0)
                {
                    items?.Invoke(list);
                    return true;
                }
        
                await Task.Yield();
            }
            Debug.LogWarning($"Search term timed out for ({typeof(U)}).");
            return false;
        }

        /// <summary>
        /// Searches for a item by type for a limited time, if it finds it returns.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <param name="appealType"></param>
        /// <param name="item"></param>
        /// <param name="searchTime"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns>True if the contains an element with the specified type, otherwise, False.</returns>
        public async Task<bool> GetSingleComponentsAsync<T,U>(T appealType, Action<U> item,float searchTime = 5f)
        {
#if MEDIATOR_STAKE_TRACE
            MediatorStackTrace.GetStackTrace("GetSingleComponentsAcync");
#endif
            float time = 0;
            if (searchTime <= 0)
            {
                Debug.LogWarning(TimeMessage);
                return false;
            }
            while (time <= searchTime)
            {
                time += Time.deltaTime;
                if(GetSingleComponent(appealType ,out U value,false) && value != null)
                {
                    item?.Invoke(value);
                    return true;
                }
                await Task.Yield();
            }
            Debug.LogWarning($"Search term timed out for ({typeof(U)}).");
            return false;
        }

        public void RegistrationObserver<T>(ObserverSingleComponent<T> observer) where T : class
        {
            observersSystem.AddObserver(observer);
        }

        public void RemoveRegistrationObserver<T>(ObserverSingleComponent<T> observer) where T : class
        {
            observersSystem.RemoveObserver(observer);
        }

        /// <summary>
        /// Adds a list of objects to the end of the ArrayList.
        /// </summary>
        /// <remarks> O(1) | O(N) </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        private int AddComponents<T>(T value) => 
            componentList.Add(value, GetComponentType(value));

        /// <summary>
        /// Adds an object to the end of the ArrayList.
        /// </summary>
        /// <remarks> O(1) | O(N) </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        private int AddSingleComponent<T>(T value,SetMode setMode = default) where T : class => 
            singleComponentList.Add(value, GetComponentType(value),observersSystem,setMode);
        
        private int AddWeakSingleComponent<T>(T value, SetMode setMode) where T : class => 
            weakSingleComponentList.Add(value, GetComponentType(value),observersSystem,setMode);

        private ComponentType GetComponentType<T>(T value)
        {
            var type = value is UnityEngine.Object ? ComponentType.UnityType : ComponentType.ObjectType;

            return type;
        }
        
        /// <summary>
        /// Finds the corresponding interface and saves it by type.
        /// </summary>
        /// <remarks> O(N) </remarks>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool TryFindInterfaceToAdd(in Type type)
        {
            foreach (var item in type.GetInterfaces())
            {
                if (item == iComponents)
                {
                    typeOfInterfaces.Add(type, MediatorInterfaces.IComponents);
                    return true;
                }
                if (item == iSingleComponent)
                {
                    typeOfInterfaces.Add(type, MediatorInterfaces.ISingleComponent);
                    return true;
                }
                if(item == iWeakSingleComponent)
                {
                    typeOfInterfaces.Add(type, MediatorInterfaces.IWeakSingleComponent);
                    return true;
                }
            }

            Debug.LogWarning($"Not implemented interface for the mediator. {type}");
            return false;
        }
        
        private int AddInternal<T>(T value,Type[] types = null,Action<Type,Type[]> permission = null,SetMode setMode = default) where T : class
        {
            var type = typeof(T);
            var indexNotFoundItems = -1;
            if (!typeOfInterfaces.ContainsKey(type))
            {
                if (!TryFindInterfaceToAdd(type))
                    return indexNotFoundItems;
            }
            typeOfInterfaces.TryGetValue(type, out MediatorInterfaces interfaces);

            switch (interfaces)
            {
                case MediatorInterfaces.IComponents:
                    permission?.Invoke(type,types);
                    return AddComponents(value);
                case MediatorInterfaces.ISingleComponent:
                    permission?.Invoke(type,types);
                    return AddSingleComponent(value,setMode);
                case MediatorInterfaces.IWeakSingleComponent:
                    permission?.Invoke(type,types);
                    return AddWeakSingleComponent(value, setMode);
                default:
                    return indexNotFoundItems;
            }
        }

        private void NotFoundComponent<T>(T obj)
        {
            if(obj is null) 
                Debug.LogWarning($"Object of type {typeof(T)} was not found in the Mediator.");
            // TODO : Future need to add functionality turning on and off logs.
        }
        

        #endregion
    }
}

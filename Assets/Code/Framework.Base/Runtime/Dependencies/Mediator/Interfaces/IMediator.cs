using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Base.Dependencies.Mediator
{
    public interface IMediator
    {
        #region Methods
        
        /// <summary>
        /// Set lock state for type.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <param name="type"></param>
        /// <param name="lockedState"></param>
        public void SetLock(in Type type,in bool lockedState);
        
        /// <summary>
        /// Adds an item by type.
        /// </summary>
        /// <remarks>  O(1) | O(N) </remarks>
        /// <typeparam name="T">class or struct</typeparam>
        /// <param name="value"></param>
        public int Add<T>(in T value,in SetMode setMode = default) where T : class;

        /// <summary>
        /// Adds item and permissions by type.
        /// </summary>
        /// <remarks> O(1) | O(N) </remarks>
        /// <typeparam name="T">class or struct</typeparam>
        /// <param name="value"></param>
        /// <param name="permissionTypes">Permissions by type.</param>
        public int Add<T>(in T value,in SetMode setMode = default, params Type[] permissionTypes) where T : class;

        /// <summary>
        /// Returns an item by type.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <typeparam name="U">class or struct</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="appealType"></param>
        /// <param name="value"></param>
        /// <returns>True if the contains an element with the specified type, otherwise, False.</returns>
        public bool GetSingleComponent<T,U>(T appealType, out U value, bool loging = true);

        /// <summary>
        /// Returns an items by type.
        /// </summary>
        /// <remarks> O(N) </remarks>
        /// <typeparam name="T">class or struct</typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="appealType"></param>
        /// <param name="value"></param>
        /// <returns>True if the contains elements with the specified type, otherwise, False.</returns>
        public bool GetComponents<T,U>(T appealType, out List<U> value, bool loging = true);
        
        /// <summary>
        /// Returns an item by type.
        /// </summary>
        /// <remarks> O(1) </remarks>
        /// <typeparam name="U">class or struct</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="appealType"></param>
        /// <param name="item"></param>
        /// <returns>True if the contains an element with the specified type, otherwise, False.</returns>
        public bool GetWeakSingleComponent<T,U>(T appealType, out U item, bool loging = true) where U : class;

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
        public Task<bool> GetComponentsAsync<T,U>(T appealType, Action<List<U>> items, float searchTime = 5f);

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
        public Task<bool> GetSingleComponentsAsync<T,U>(T appealType, Action<U> item, float searchTime = 5f);

        #endregion
    }

    public interface IMediatorObserver : IMediator
    {
        public void RegistrationObserver<T>(ObserverSingleComponent<T> observerSingle) where T : class;

        public void RemoveRegistrationObserver<T>(ObserverSingleComponent<T> observerSingle) where T : class;
    }
}
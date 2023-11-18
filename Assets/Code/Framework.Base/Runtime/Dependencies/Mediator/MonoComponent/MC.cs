using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework.Base.Dependencies.Mediator
{
    /// <summary>
    /// Mediator component
    /// </summary>
    public class MC : MonoBehaviour 
    {
        #region Fields

        private static IMediator instance;
        private static IMediatorObserver instanceObserver;

        public bool dontDestroyOnLoadOnEnable;
        public bool removeMediatorOnDestroy; 

        #endregion
        

        #region Properties

        public static IMediator Instance
        {
            get
            {
                if (instance == null) Init();
                return instance;
            }
        }
        
        internal static IMediatorObserver InstanceObserver
        {
            get
            {
                if (instance == null) Init();
                if (instanceObserver != null) 
                    return instanceObserver;
                throw new NotImplementedException($"Not implemented {nameof(IMediatorObserver)} interfaces.");
            }
        }


        #endregion
        

        #region Unity Lifecycle

        private void OnEnable()
        {
            if (dontDestroyOnLoadOnEnable)
            {
                DontDestroyOnLoad(this);
            }
        }

        private void OnDestroy()
        {
            if (removeMediatorOnDestroy)
            {
                instance = null;
            }
        }

        #endregion
        

        #region Methods
        
        public static void SetLock(in Type type,in bool lockedState) => Instance.SetLock(type,lockedState);

        public static int Add<T>(in T value,in SetMode setMode = SetMode.None) where T : class => Instance.Add(value, setMode);

        public static int Add<T>(in T value, in SetMode setMode = SetMode.None, params Type[] permissionTypes) where T : class => 
            Instance.Add(value, setMode);

        public static bool GetSingleComponent<T, U>(T appealType, out U value, bool loging = true) => 
            Instance.GetSingleComponent(appealType, out value, loging);

        public static bool GetComponents<T, U>(T appealType, out List<U> value, bool loging = true) => 
            Instance.GetComponents(appealType, out value, loging);

        public static bool GetWeakSingleComponent<T, U>(T appealType, out U item, bool loging = true) where U : class => 
            Instance.GetWeakSingleComponent(appealType, out item, loging);

        public static Task<bool> GetComponentsAsync<T, U>(T appealType, Action<List<U>> items, float searchTime = 5) => 
            Instance.GetComponentsAsync(appealType, items, searchTime);

        public static Task<bool> GetSingleComponentsAsync<T, U>(T appealType, Action<U> item, float searchTime = 5) => 
            Instance.GetSingleComponentsAsync(appealType, item, searchTime);

        public static void Init<T>(T mediator) where T : IMediator
        {
            instance = mediator;
            instanceObserver = mediator as IMediatorObserver;
        }

        public static void Init() => instance = instanceObserver = new MediatorSystem();

        #endregion
    }
}

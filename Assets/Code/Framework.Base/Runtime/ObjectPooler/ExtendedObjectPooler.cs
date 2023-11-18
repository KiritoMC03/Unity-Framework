using System;
using System.Collections.Generic;
using Framework.Base.Dependencies.Mediator;
using Framework.Base.ObjectPool;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Idlers
{
    public sealed class ExtendedObjectPooler : IObjectPooler, ISingleComponent, IDisposable
    {
        #region Fields

        private HashSet<GameObject> retrievedFromPoolObjects;
        private bool returnDontDestroyOnSceneChange;

        #endregion
        
        #region Properties
        
        public bool UseWrapperFunctions { get; set; } = true;

        public bool ReturnDontDestroyOnSceneChange
        {
            get => returnDontDestroyOnSceneChange;
            set
            {
                returnDontDestroyOnSceneChange = value;
                retrievedFromPoolObjects = value ? new HashSet<GameObject>() : default;
                if (value) SceneManager.activeSceneChanged += HandleActiveSceneChanged;
                else SceneManager.activeSceneChanged -= HandleActiveSceneChanged;
            }
        }

        #endregion

        #region Constructors

        public ExtendedObjectPooler(bool returnDontDestroyOnSceneChange = false)
        {
            ReturnDontDestroyOnSceneChange = returnDontDestroyOnSceneChange;
        }

        #endregion

        #region Methods

        public void Disable()
        {
            UseWrapperFunctions = false;
            ReturnDontDestroyOnSceneChange = false;
        }

        private void HandleActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            foreach (GameObject obj in retrievedFromPoolObjects) 
                TrySendToPool(obj);
            retrievedFromPoolObjects.Clear();
        }

        #endregion

        #region IObjectPooler

        public GameObject GetObject(PooledObjectType type)
        {
            GameObject result = ObjectPooler.Instance.GetObject(type);
            if (UseWrapperFunctions && ReturnDontDestroyOnSceneChange) retrievedFromPoolObjects.Add(result);
            return result;
        }

        public bool TrySendToPool(GameObject obj)
        {
            if (UseWrapperFunctions && ReturnDontDestroyOnSceneChange) retrievedFromPoolObjects.Remove(obj);
            bool result = ObjectPooler.Instance.TrySendToPool(obj);
            return result;
        }

        #endregion

        #region IDisposable

        public void Dispose() => Disable();

        #endregion
    }
}
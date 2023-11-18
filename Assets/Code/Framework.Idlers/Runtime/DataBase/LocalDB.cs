using System.Collections.Generic;
using Framework.Base.SaveLoad;
using UnityEngine;

namespace Framework.Idlers.LocalDB
{
    public class LocalDB
    {
        #region Fields

        private ProjectData projectData;

        #endregion

        #region Constructors

        public LocalDB()
        {
            if (SLComponent.Instance.TryLoad(ref projectData)) 
                Debug.Log("Project Data loaded");
            else projectData = new ProjectData(new List<object>());
        }

        #endregion

        #region Methods

        public void AddDB<T>(T db) => projectData.databases.Add(db);

        public void TryAddDB<T>(T db) 
        {
            if (!Has<T>())
                AddDB(db);
        }

        public bool RemoveDB<T>(T db)
        {
            return projectData.databases.Remove(db);
        }

        public T GetDB<T>()
        {
            int length = projectData.databases.Count;
            for (int i = 0; i < length; i++)
                if (projectData.databases[i] is T result)
                    return result;

            Debug.LogError($"DataBase of type {typeof(T)} not fount!");
            return default;
        }

        public bool Has<T>()
        {
            int length = projectData.databases.Count;
            for (int i = 0; i < length; i++)
                if (projectData.databases[i] is T)
                    return true;

            return false;
        }

        public IEnumerable<object> IterateDB() => projectData.databases;

        public void HandleApplicationQuit() => Save();
        public void HandleApplicationPause(bool isPause)
        {
            if (isPause) Save();
        }

        private void Save() => SLComponent.Instance.TrySave(ref projectData);

        #endregion
    }
}
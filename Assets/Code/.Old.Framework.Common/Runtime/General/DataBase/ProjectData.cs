using System;
using System.Collections.Generic;
using General;
using General.SaveLoad;

namespace GameKit.General.LocalDB
{
    [Serializable] [Data("unityconfig.slsy", SaveLoadType.Json)]
    public struct ProjectData : ISaveLoadCallbackReceiver
    {
        public List<object> databases;

        public ProjectData(List<object> databases)
        {
            this.databases = databases;
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            databases ??= new List<object>();
        }
    }
}
using System;
using General;
using UnityEngine;

namespace GameKit.CraftModule.LevelsSystem
{
    [Serializable]
    public class LevelCreatingByPrefabsPreferences
    {
        public SerializedInterfacesList<ILevelDefault> levelsPrefabs;
        [Tooltip("Can be null (will use root)")]
        public Transform levelParent;
    }
}
using System;
using Framework.Base.Collections;
using UnityEngine;

namespace Framework.Idlers.LevelsSystem
{
    [Serializable]
    public class LevelCreatingByPrefabsPreferences
    {
        public SerializedInterfacesList<ILevelDefault> levelsPrefabs;
        [Tooltip("Can be null (will use root)")]
        public Transform levelParent;
    }
}
using System;
using General;
using GameKit.General.Structures;
using UnityEngine;

namespace GameKit.CraftModule.Resource
{
    public class StringBasedIdentifiersConfig : ScriptableObject
    {
        public SerializedHashSet<string> typesList;

        public static string GetAssetFileName(Type stringBasedIdentifierType)
        {
            if (!stringBasedIdentifierType.IsSubclassOf(typeof(StringBasedIdentifier)))
                throw new ArgumentException(
                    $"{stringBasedIdentifierType} Type is not subclass of {typeof(StringBasedIdentifier)}");
            return $"{stringBasedIdentifierType.Name}Asset.asset";
        }
    }
}
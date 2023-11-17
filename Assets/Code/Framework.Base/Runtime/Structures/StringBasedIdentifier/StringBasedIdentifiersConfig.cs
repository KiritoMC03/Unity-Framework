using System;
using Framework.Base.Collections;
using UnityEngine;

namespace Framework.Base
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
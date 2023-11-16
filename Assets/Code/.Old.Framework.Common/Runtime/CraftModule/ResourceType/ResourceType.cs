using System;
using GameKit.General.Structures;

namespace GameKit.CraftModule.Resource
{
    [Serializable]
    public class ResourceType : StringBasedIdentifier, IResourceType
    {
        #region Properties

        public string Value => value;

        #endregion

        #region Constructors

        public ResourceType(string value) : base(value)
        {
        }

        #endregion
    }
}
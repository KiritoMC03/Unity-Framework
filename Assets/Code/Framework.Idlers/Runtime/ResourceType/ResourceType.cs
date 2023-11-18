using System;
using Framework.Base;

namespace Framework.Idlers.Resource
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
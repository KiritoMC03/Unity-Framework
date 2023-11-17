#if UNITY_EDITOR
using UnityEngine;

namespace Framework.Base.Editor.EnumCreator
{
    [CreateAssetMenu(fileName = "EnumCreatorConfigAsset", menuName = "EnumCreator/New Config", order = 0)]
    public class EnumCreatorConfig : ScriptableObject
    {
        #region Fields

        public string csFileName;
        public bool useNamespace;
        public bool useDefines;
        public string targetDefine;
        public string targetNamespace;

        [Tooltip("In checking the existing instances of enum these paths will be ignored")]
        public string[] ignoreExistInPaths;

        #endregion
    }
}
#endif
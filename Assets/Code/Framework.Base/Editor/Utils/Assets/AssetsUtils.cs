using System.IO;

namespace Framework.Base.Utils.Editor
{
    public static class AssetsUtils
    {
        public static string LocalAssetPathToFullPath(string assetPath)
        {
            DirectoryInfo directory = Directory.GetParent(UnityEngine.Application.dataPath);
            return directory?.ToString().Replace('\\', '/') + "/" + assetPath;
        }
    }
}
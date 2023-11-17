using UnityEngine;

namespace Framework.Base.CSV
{
    public class CSVConfig : ScriptableObject
    {
        public TextAsset textAsset;
        public Rect header;
        public Rect body;
        public bool headerStatus;
        public bool bodyStatus;
        public string configName;
        public string assemblyName;
    }
}
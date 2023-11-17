using System;
using System.IO;
using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Base.SaveLoad
{
    internal struct Json : ISaveLoad
    {
        #region Properties

        public ISaveLoad Converter { get; set; }

        #endregion


        #region ISaveLoad

        public bool TrySave<T>(ref T data,
            string fileName,
            IEncryption encryption,
            PlayerPrefsMode playerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson) where T : struct
        {
            try
            {
                string text = encryption.Encode(JsonUtility.ToJson(data));
                File.WriteAllText(SLPath.GetPath(fileName), text);
                return true;
            }
            catch (Exception ex)
            {
                ex.Debug();
                return false;
            }
        }

        public bool TryLoad<T>(ref T data,
            string fileName,
            IEncryption encryption,
            PlayerPrefsMode playerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson) where T : struct
        {
            if (File.Exists(SLPath.GetPath(fileName)))
                try
                {
                    string text = File.ReadAllText(SLPath.GetPath(fileName));
                    encryption.Decode(text, out string json);
                    data = JsonUtility.FromJson<T>(json);
                    return true;
                }
                catch (Exception ex)
                {
                    ex.Debug();
                    data = new T();
                    return false;
                }

            data = new T();
            return false;
        }

        #endregion
    }
}
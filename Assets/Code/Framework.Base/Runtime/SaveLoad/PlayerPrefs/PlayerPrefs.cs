using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.ComponentModel;
using General.Extensions;

namespace General.SaveLoad
{
    internal struct PlayerPrefs : ISaveLoad
    {
        #region Fields

        private const char separator = '/';

        private List<PlayerPrefsData> playerPrefsDatas;

        #endregion


        #region Properties

        public ISaveLoad Converter { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Saves data using PlayerPrefs
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        private void Save(in string key, in string value)
        {
            UnityEngine.PlayerPrefs.SetString(key, value);
        }

        /// <summary>
        /// Load data using PlayerPrefs
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">Out Value</param>
        /// <returns>Status</returns>
        private bool Load(in string key, out string value)
        {
            if (UnityEngine.PlayerPrefs.HasKey(key))
            {
                value = UnityEngine.PlayerPrefs.GetString(key);
                return true;
            }
            else
            {
                value = "";
                return false;
            }
        }

        private void FindFields<T>(ref T data, string fileName, bool IsSave = true) where T : struct
        {
            FieldInfo[] filds = GetFields(ref data);
            playerPrefsDatas = new List<PlayerPrefsData>();
            foreach (FieldInfo item in filds)
            {
                bool status = true;

                foreach (Attribute i in item.GetCustomAttributes())
                    if (i.GetType() == typeof(NonSerializedAttribute))
                        status = false;

                if (status)
                {
                    StringBuilder key = new StringBuilder();
                    key.Append(item.FieldType.ToString())
                        .Append(separator) //
                        .Append(fileName)
                        .Append(separator) //
                        .Append(item.Name);
                    string value;
                    if (IsSave) // save
                        value = item.GetValue(data)?.ToString() ?? string.Empty;
                    else // load
                        value = "0";

                    playerPrefsDatas.Add(new PlayerPrefsData(key.ToString(), value));
                }
            }
        }


        private void SetFieldValue<T>(ref T data, object dataObject, string fildName, string value) where T : struct
        {
            FieldInfo[] filds = GetFields(ref data);

            foreach (FieldInfo item in filds)
                if (fildName == item.Name)
                {
                    string type = item.FieldType.ToString();

                    if (TryParse(value, item.SetValue, type, dataObject)) break;
                }
        }

        private bool TryParse(string input, Action<object, object> action, string type, object data)
        {
            try
            {
                if (type == input.GetType().ToString())
                {
                    action?.Invoke(data, input);
                    return true;
                }
                else
                {
                    Type typeLocal = Type.GetType(type);
                    TypeConverter converter = TypeDescriptor.GetConverter(typeLocal);

                    if (converter != null) action?.Invoke(data, converter.ConvertFromString(input));
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.Debug();
                return false;
            }
        }

        private FieldInfo[] GetFields<T>(ref T data) where T : struct
        {
            Type type = data.GetType();
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public);
        }

        #endregion


        #region ISaveLoad

        public bool TrySave<T>(ref T data,
            string fileName,
            IEncryption encryption,
            PlayerPrefsMode playerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson) where T : struct
        {
            switch (playerPrefsMode)
            {
                case PlayerPrefsMode.PlayerPrefs:
                    FindFields(ref data, fileName);
                    foreach (PlayerPrefsData item in playerPrefsDatas) Save(item.Kay, item.Value);
                    return true;

                case PlayerPrefsMode.PlayerPrefsToJson:
                    try
                    {
                        string text = encryption.Encode(JsonUtility.ToJson(data));
                        Save(fileName, text);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ex.Debug();
                        return false;
                    }

                default:
                    return false;
            }
        }

        public bool TryLoad<T>(ref T data,
            string fileName,
            IEncryption encryption,
            PlayerPrefsMode playerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson) where T : struct
        {
            switch (playerPrefsMode)
            {
                case PlayerPrefsMode.PlayerPrefs:
                    FindFields(ref data, fileName, false);
                    object dataObject = data;
                    foreach (PlayerPrefsData item in playerPrefsDatas)
                    {
                        string[] s = item.Kay.Split(separator);
                        if (Load(item.Kay, out string value))
                            SetFieldValue(ref data, dataObject, s[s.Length - 1], value);
                    }

                    data = (T)dataObject;

                    return true;

                case PlayerPrefsMode.PlayerPrefsToJson:
                    try
                    {
                        Load(fileName, out string text);
                        encryption.Decode(text, out string json);
                        data = JsonUtility.FromJson<T>(json);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        data = new T();
                        ex.Debug();
                        return false;
                    }

                default:
                    return false;
            }
        }

        #endregion
    }
}
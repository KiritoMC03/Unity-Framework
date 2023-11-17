using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Base.SaveLoad
{
    public sealed class SaveLoadSystem : ISaveLoadSystem
    {
        #region Fields

        private Dictionary<Type, Data> types = new Dictionary<Type, Data>();
        private Dictionary<SaveLoadType, ISaveLoad> saveLoad = new Dictionary<SaveLoadType, ISaveLoad>(3);
        private Dictionary<EncryptionType, IEncryption> encryption = new Dictionary<EncryptionType, IEncryption>(2);

        #endregion


        #region Methods

        // TODO : SaveLoad - Global settings

        private bool IsTypeRegistered<T>(T data, out Data dataAttribute) where T : struct =>
            types.TryGetValue(data.GetType(), out dataAttribute);

        private bool HasAttribute<T>(ref T data, out Data dataAttribute) where T : struct
        {
            DataAttribute attribute =
                (DataAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(DataAttribute));

            if (attribute == null)
            {
                Debug.LogWarning("The attribute was not found.");
                dataAttribute = new Data();
                return false;
            }
            else
            {
                dataAttribute = new Data();
                dataAttribute.FileName = attribute.FileName;
                dataAttribute.SaveLoadType = attribute.SaveLoadType;
                dataAttribute.EncryptionType = attribute.EncryptionType;
                dataAttribute.PlayerPrefsMode = attribute.PlayerPrefsMode;
                dataAttribute.ConstructorID = attribute.ConstructorID;
                FactoryEncryption(dataAttribute.EncryptionType);
                FactorySaveLoad(dataAttribute.SaveLoadType);
                types.Add(data.GetType(), dataAttribute);
                return true;
            }
        }

        private bool Save<T>(ref T data, in Data dataAttribute) where T : struct
        {
            if (saveLoad.TryGetValue(dataAttribute.SaveLoadType, out ISaveLoad value))
            {
                encryption.TryGetValue(dataAttribute.EncryptionType, out IEncryption encrypt);
                if (data is ISaveLoadCallbackReceiver callbackReceiver)
                {
                    callbackReceiver.OnBeforeSerialize();
                    data = (T)callbackReceiver;
                }

                if (dataAttribute.SaveLoadType == SaveLoadType.PlayerPrefs)
                {
                    saveLoad.TryGetValue(SaveLoadType.Json, out ISaveLoad converter);
                    value.Converter = converter;
                    return value.TrySave(ref data, dataAttribute.FileName, encrypt, dataAttribute.PlayerPrefsMode);
                }
                else
                {
                    return value.TrySave(ref data, dataAttribute.FileName, encrypt);
                }
            }

            return false;
        }

        private bool Load<T>(ref T data, in Data dataAttribute) where T : struct
        {
            if (saveLoad.TryGetValue(dataAttribute.SaveLoadType, out ISaveLoad value))
            {
                encryption.TryGetValue(dataAttribute.EncryptionType, out IEncryption encrypt);
                bool result;
                if (dataAttribute.SaveLoadType == SaveLoadType.PlayerPrefs)
                {
                    saveLoad.TryGetValue(SaveLoadType.Json, out ISaveLoad converter);
                    value.Converter = converter;
                    result = value.TryLoad(ref data, dataAttribute.FileName, encrypt, dataAttribute.PlayerPrefsMode);
                }
                else
                {
                    result = value.TryLoad(ref data, dataAttribute.FileName, encrypt);
                }

                if (result && data is ISaveLoadCallbackReceiver callbackReceiver)
                {
                    callbackReceiver.OnAfterDeserialize();
                    data = (T)callbackReceiver;
                }

                return result;
            }

            return false;
        }

        private void FactoryEncryption(in EncryptionType encryptionType)
        {
            switch (encryptionType)
            {
                case EncryptionType.None:
                    if (!encryption.ContainsKey(encryptionType)) encryption.Add(encryptionType, new NoneEncryption());
                    break;

                case EncryptionType.Binary:
                    if (!encryption.ContainsKey(encryptionType)) encryption.Add(encryptionType, new BinaryEncryption());
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Creates save mode ( PlayerPrefs, Json, Xml)
        /// </summary>
        /// <param name="saveLoadType">Data mode</param>
        /// <exception cref="NotImplementedException">
        /// throws an exception when there is no implementation</exception>
        private void FactorySaveLoad(in SaveLoadType saveLoadType)
        {
            switch (saveLoadType)
            {
                case SaveLoadType.PlayerPrefs:
                    if (!saveLoad.ContainsKey(saveLoadType)) saveLoad.Add(saveLoadType, new PlayerPrefs());
                    goto case SaveLoadType.Json;

                case SaveLoadType.Json:
                    if (!saveLoad.ContainsKey(saveLoadType)) saveLoad.Add(saveLoadType, new Json());
                    break;

                case SaveLoadType.Xml:
                    if (!saveLoad.ContainsKey(saveLoadType))
                    {
                        saveLoad.Add(saveLoadType, new Xml());
                        ;
                    }

                    break;

                default:
                    break;
            }
        }

        #endregion


        #region ISaveLoadSystem

        public bool TryLoad<T>(ref T data) where T : struct
        {
            if (IsTypeRegistered(data, out Data dataAttribute))
            {
                return Load(ref data, dataAttribute);
            }
            else
            {
                if (HasAttribute(ref data, out dataAttribute)) return Load(ref data, dataAttribute);
            }

            return false;
        }

        public bool TrySave<T>(ref T data) where T : struct
        {
            if (IsTypeRegistered(data, out Data dataAttribute))
            {
                return Save(ref data, dataAttribute);
            }
            else
            {
                if (HasAttribute(ref data, out dataAttribute)) return Save(ref data, dataAttribute);
            }

            return false;
        }

        #endregion
    }
}
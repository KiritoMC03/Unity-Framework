using System;

namespace General
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class DataAttribute : Attribute
    {
        #region Fields

        public readonly string FileName;
        public readonly SaveLoadType SaveLoadType;
        public readonly EncryptionType EncryptionType;
        public readonly PlayerPrefsMode PlayerPrefsMode;

        #endregion


        #region Properties

        public int ConstructorID { get; private set; }

        #endregion


        #region Methods

        public DataAttribute(string fileName)
        {
            ConstructorID = 1;
            FileName = fileName;
            SaveLoadType = SaveLoadType.Json;
            EncryptionType = EncryptionType.None;
            PlayerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson;
        }

        public DataAttribute(string fileName, SaveLoadType saveLoadType)
        {
            ConstructorID = 2;
            FileName = fileName;
            SaveLoadType = saveLoadType;
            EncryptionType = EncryptionType.None;
            PlayerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson;
        }

        public DataAttribute(string fileName, SaveLoadType saveLoadType, EncryptionType encryptionType)
            : this(fileName, saveLoadType)
        {
            ConstructorID = 3;
            EncryptionType = encryptionType;
            PlayerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson;
        }

        public DataAttribute(string fileName, SaveLoadType saveLoadType, EncryptionType encryptionType,
            PlayerPrefsMode playerPrefsMode)
            : this(fileName, saveLoadType, encryptionType)
        {
            ConstructorID = 4;
            PlayerPrefsMode = playerPrefsMode;
        }

        #endregion
    }
}
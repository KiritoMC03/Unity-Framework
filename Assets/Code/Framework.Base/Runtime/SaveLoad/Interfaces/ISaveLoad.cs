namespace Framework.Base.SaveLoad
{
    internal interface ISaveLoad
    {
        #region Properties

        ISaveLoad Converter { get; set; }

        #endregion

        #region Methods

        bool TrySave<T>(ref T data,
            string fileName,
            IEncryption encryption,
            PlayerPrefsMode playerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson) where T : struct;

        bool TryLoad<T>(ref T data,
            string fileName,
            IEncryption encryption,
            PlayerPrefsMode playerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson) where T : struct;

        #endregion
    }
}
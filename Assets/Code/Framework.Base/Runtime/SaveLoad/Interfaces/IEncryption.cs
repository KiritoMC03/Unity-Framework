namespace Framework.Base.SaveLoad
{
    internal interface IEncryption
    {
        #region Methods

        public string[] Encode(in string[] input);

        public string Encode(in string input);

        public bool Decode(in string[] input, out string[] output);

        public bool Decode(in string input, out string output);

        #endregion
    }
}
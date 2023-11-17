namespace Framework.Base.SaveLoad
{
    internal struct NoneEncryption : IEncryption
    {
        #region IEncryption

        public bool Decode(in string[] input, out string[] output)
        {
            output = input;
            return true;
        }

        public bool Decode(in string input, out string output)
        {
            output = input;
            return true;
        }

        public string[] Encode(in string[] input) => input;

        public string Encode(in string input) => input;

        #endregion
    }
}
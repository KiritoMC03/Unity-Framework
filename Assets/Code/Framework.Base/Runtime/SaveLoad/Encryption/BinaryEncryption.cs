using System;
using System.Text;
using Framework.Base.Extensions;

namespace Framework.Base.SaveLoad
{
    internal struct BinaryEncryption : IEncryption
    {
        #region IEncryption

        public bool Decode(in string[] input, out string[] output)
        {
            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    byte[] bytes = new byte[input[i].Length / 2];
                    for (int j = 0; j < input[i].Length; j += 2)
                        bytes[j / 2] = Convert.ToByte(input[i].Substring(j, 2), 16);
                    input[i] = Encoding.Unicode.GetString(bytes);
                }

                output = input;
                return true;
            }
            catch (Exception ex)
            {
                ex.Debug();
                output = Array.Empty<string>();
                return false;
            }
        }

        public bool Decode(in string input, out string output)
        {
            try
            {
                byte[] bytes = new byte[input.Length / 2];
                for (int i = 0; i < input.Length; i += 2) bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);

                output = Encoding.Unicode.GetString(bytes);
                return true;
            }
            catch (Exception ex)
            {
                ex.Debug();
                output = string.Empty;
                return false;
            }
        }

        public string Encode(in string input)
        {
            byte[] by = Encoding.Unicode.GetBytes(input);
            return BitConverter.ToString(by).Replace("-", "");
        }

        public string[] Encode(in string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                byte[] by = Encoding.Unicode.GetBytes(input[i]);
                input[i] = BitConverter.ToString(by).Replace("-", "");
            }

            return input;
        }

        #endregion
    }
}
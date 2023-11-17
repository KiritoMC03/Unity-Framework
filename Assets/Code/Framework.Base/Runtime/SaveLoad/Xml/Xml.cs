using System;
using System.IO;
using System.Xml.Serialization;
using Framework.Base.Extensions;

namespace Framework.Base.SaveLoad
{
    internal struct Xml : ISaveLoad
    {
        #region Fields

        private string fileName;
        private IEncryption encryption;

        #endregion


        #region Properties

        public ISaveLoad Converter { get; set; }

        #endregion


        #region Methods

        private void Encode()
        {
            string[] allLines = File.ReadAllLines(SLPath.GetPath(fileName));
            allLines = encryption.Encode(allLines);
            File.WriteAllLines(SLPath.GetPath(fileName), allLines);
        }

        private void Decode()
        {
            string[] allLines = File.ReadAllLines(SLPath.GetPath(fileName));
            encryption.Decode(allLines, out allLines);
            File.WriteAllLines(SLPath.GetPath(fileName), allLines);
        }

        #endregion


        #region ISaveLoad

        public bool TrySave<T>(ref T data,
            string fileName,
            IEncryption encryption,
            PlayerPrefsMode playerPrefsMode = PlayerPrefsMode.PlayerPrefsToJson) where T : struct
        {
            this.fileName = fileName;
            this.encryption = encryption;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamWriter writer = new StreamWriter(SLPath.GetPath(fileName)))
                {
                    serializer.Serialize(writer.BaseStream, data);
                    writer.Close();
                }

                Encode();
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
            this.fileName = fileName;
            this.encryption = encryption;

            try
            {
                Decode();

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(SLPath.GetPath(fileName)))
                {
                    data = (T)serializer.Deserialize(reader.BaseStream);
                    reader.Close();
                }

                Encode();
                return true;
            }
            catch (Exception ex)
            {
                ex.Debug();
                return false;
            }
        }

        #endregion
    }
}
using NUnit.Framework;
using UnityEngine;
using General;
using General.SaveLoad;
using UnityEditor;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace SaveLoad.Test
{
    public class SaveLoadTest
    {
        [Test]
        public void EncryptionBinaryString()
        {
            IEncryption encryption = new BinaryEncryption();
            var helloSaveLoadSystem = "Hello SaveLoad System.";
            var en =  encryption.Encode(helloSaveLoadSystem);
            var state = encryption.Decode(en, out string de);
            if(de == helloSaveLoadSystem && state) Assert.Pass();
            Assert.Fail();
        }
        
        [Test]
        public void EncryptionBinaryStringArray()
        {
            IEncryption encryption = new BinaryEncryption();
            var helloSaveLoadSystem = "Hello SaveLoad System.";
            var helloSaveLoadSystem1 = "Hello SaveLoad System1.";
            var array = new string[] {helloSaveLoadSystem, helloSaveLoadSystem1};
            var en =  encryption.Encode(array);
            var state = encryption.Decode(en, out string[] de);
            var check = false; 
            for (int i = 0; i < de.Length; i++)
            {
                check = true;
                if(de[i] == array[i]) continue;
                check = false;
                break;
            }
            if(state && check) Assert.Pass();
            Assert.Fail();
        }
        
        [Test]
        public void JsonTrySaveAndTryLoad()
        {
            ISaveLoadSystem system = new SaveLoadSystem();
            var jsonTest = new JsonTest(100);
            var savesState = system.TrySave(ref jsonTest);
            var jsonTest1 = new JsonTest(0);
            var loadState = system.TryLoad(ref jsonTest1);
            if (savesState && loadState && jsonTest.Num == jsonTest1.Num)
            {
                Delete(JsonTest.JsonTestPath);
                Assert.Pass();
            }
            Delete(JsonTest.JsonTestPath);
            Assert.Fail();
        }
        
        [Test]
        public void PlayerPrefsTrySaveAndTryLoad()
        {
            ISaveLoadSystem system = new SaveLoadSystem();
            var playerPrefsTest = new PlayerPrefsTest(100);
            var savesState = system.TrySave(ref playerPrefsTest);
            var playerPrefsTest1 = new PlayerPrefsTest(0);
            var loadState = system.TryLoad(ref playerPrefsTest1);
            if (savesState && loadState && playerPrefsTest.Num == playerPrefsTest1.Num)
            {
                DeletePlayerPrefs(PlayerPrefsTest.PlayerPrefsKey);
                Assert.Pass();
            }
            DeletePlayerPrefs(PlayerPrefsTest.PlayerPrefsKey);
            Assert.Fail();
        }
        
        [Test]
        public void XMLTrySaveAndTryLoad()
        {
            ISaveLoadSystem system = new SaveLoadSystem();
            var xmlTest = new XMLTest(100);
            var savesState = system.TrySave(ref xmlTest);
            var xmlTest1 = new  XMLTest(0);
            var loadState = system.TryLoad(ref xmlTest1);
            if (savesState && loadState && xmlTest.Num == xmlTest1.Num)
            {
                Delete(XMLTest.XMLPath);
                Assert.Pass();
            }
            Delete(XMLTest.XMLPath);
            Assert.Fail();
        }

        private void Delete(string name)
        {
            AssetDatabase.Refresh();
            if(AssetDatabase.DeleteAsset(name))
                Debug.Log($"Delete {name}");
        }
        
        private void DeletePlayerPrefs(string key)
        {
            AssetDatabase.Refresh();
            PlayerPrefs.DeleteKey(key);
            Debug.Log($"Delete {key}");
        }
    }

    [General.Data(JsonTestTxt,SaveLoadType.Json,EncryptionType.None)]
    internal struct JsonTest
    {
        public const string JsonTestTxt = "JsonTest.txt";
        public const string JsonTestPath = "Assets/JsonTest.txt";
        public int Num;

        public JsonTest(int num)
        {
            Num = num;
        }
    }
    
    [General.Data(PlayerPrefsKey,SaveLoadType.PlayerPrefs,EncryptionType.None)]
    internal struct PlayerPrefsTest
    {
        public const string PlayerPrefsKey = "Test";
        public int Num;

        public PlayerPrefsTest(int num)
        {
            Num = num;
        }
    }
    
    [General.Data(XMLTxt,SaveLoadType.Xml,EncryptionType.None)]
    public struct XMLTest
    {
        public const string XMLTxt = "Test.txt";
        public const string XMLPath = "Assets/Test.txt";
        
        public int Num;

        public XMLTest(int num)
        {
            Num = num;
        }
    }
}

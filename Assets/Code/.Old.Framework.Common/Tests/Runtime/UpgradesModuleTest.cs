using System.Collections.Generic;
using GameKit.UpgradeModule;
using NUnit.Framework;
using UnityEngine;

namespace GameKit.Runtime.Test
{
    public class UpgradesModuleTest
    {
        #region Methods

        [Test]
        public void GetUpgradableLevelInt() => GetUpgradableLevelGeneric<int>(1);
        
        [Test]
        public void GetUpgradableLevelString() => GetUpgradableLevelGeneric<string>("strId");
        
        [Test]
        public void GetUpgradableLevelFloat() => GetUpgradableLevelGeneric<float>(1.1f);
        
        [Test]
        public void GetUpgradableLevelDouble() => GetUpgradableLevelGeneric<double>(1.1d);
        
        [Test]
        public void GetUpgradableLevelObject() => GetUpgradableLevelGeneric<object>(new object());

        [Test]
        public void UpgradeInt() => UpgradeGeneric<int>(1, 10);
        
        [Test]
        public void UpgradeString() => UpgradeGeneric<string>("strId", 11);
        
        [Test]
        public void UpgradeFloat() => UpgradeGeneric<float>(1.1f, 12);
        
        [Test]
        public void UpgradeDouble() => UpgradeGeneric<double>(1.1d, 13);
        
        [Test]
        public void UpgradeObject() => UpgradeGeneric<object>(new object(), 14);

        private void GetUpgradableLevelGeneric<T>(T identifier)
        {
            Dictionary<T, int> levels = new Dictionary<T, int>();
            UpgradeSystem<T> system = new UpgradeSystem<T>(levels);
            UpgradableTest obj = new UpgradableTest();
            int customLevel = 0;
            system.Add(identifier, obj, customLevel);
            system.TryGetLevel(identifier, out int level);
            Assert.IsTrue(level == customLevel);
        }

        private void UpgradeGeneric<T>(T identifier, int level)
        {
            Dictionary<T, int> levels = new Dictionary<T, int>();
            UpgradeSystem<T> system = new UpgradeSystem<T>(levels);
            UpgradableTest obj = new UpgradableTest();
            system.Add(identifier, obj);
            system.TryUpgrade(identifier, level);
            Assert.IsTrue(level == obj.level);
        }

        #endregion
    }
    
    public class UpgradableTest : IUpgradable
    {
        public int level;

        public void Upgrade(int level)
        {
            this.level = level;
        }
    }
}
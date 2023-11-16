using System.Collections.Generic;
using UnityEngine;
using General.Extensions;
using General.Mediator;

namespace GameKit.UpgradeModule
{
    /// <typeparam name="TId">Identifier type</typeparam>
    public class UpgradeSystem<TId> : IUpgradeSystem<TId>
    {
        #region Fields

        private Dictionary<TId, IUpgradable> registeredUpgradables = new Dictionary<TId, IUpgradable>(20);
        private Dictionary<TId, int> upgradablesLevels;
        private int baseLevel = 0;

        private const string IdentifierIsNull = "Identifier is null.";
        private const string UpgradableNotFound = "IUpgradable object not found.";

        #endregion

        #region Constructors

        public UpgradeSystem(Dictionary<TId, int> upgradablesLevels, int baseLevel = 0)
        {
            this.upgradablesLevels = upgradablesLevels;
            this.baseLevel = baseLevel;
        }

        #endregion
        
        #region Methods

        public AddUpgradableResult Add(TId identifier, IUpgradable upgradable, SetMode setMode = SetMode.None) =>
            Add(identifier, upgradable, baseLevel, setMode);

        public AddUpgradableResult Add(TId identifier, IUpgradable upgradable, int customBaseLevel, SetMode setMode = SetMode.None)
        {
            if (CheckIdentifierIsNull(identifier)) return AddUpgradableResult.Failed;
            
            if (registeredUpgradables.ContainsKey(identifier))
            {
                if (setMode == SetMode.Force)
                    registeredUpgradables[identifier] = upgradable;
                else Debug.LogWarning($"{typeof(UpgradeSystem<TId>)} already contains object with identifier: {identifier}");
            }
            else 
                registeredUpgradables.Add(identifier, upgradable);

            if (upgradablesLevels.ContainsKey(identifier))
                return AddUpgradableResult.AddedAndLevelAlreadyContains;

            upgradablesLevels.Add(identifier, customBaseLevel);
            return AddUpgradableResult.Added;
        }

        public bool TryUpgrade(TId identifier)
        {
            if (TryGet(identifier, out IUpgradable upgradable))
            {
                upgradable.Upgrade(++upgradablesLevels[identifier]);
                return true;
            }

            Debug.LogWarning(UpgradableNotFound);
            return false;
        }
        
        public bool TryUpgrade(TId identifier, int level)
        {
            if (TryGet(identifier, out IUpgradable upgradable))
            {
                upgradablesLevels[identifier] = level;
                upgradable.Upgrade(level);
                return true;
            }

            Debug.LogWarning(UpgradableNotFound);
            return false;
        }

        public bool TryActualizeLevel(TId identifier)
        {
            if (!upgradablesLevels.ContainsKey(identifier))
                return false;

            return TryUpgrade(identifier, upgradablesLevels[identifier]);
        }
        
        public bool TryGetLevel(TId identifier, out int level)
        {
            level = -1;
            if (CheckIdentifierIsNull(identifier)) return false;
            return upgradablesLevels.TryGetValue(identifier, out level);
        }

        private bool TryGet(TId identifier, out IUpgradable upgradable)
        {
            upgradable = default;
            if (CheckIdentifierIsNull(identifier)) return false;
            
            return registeredUpgradables.TryGetValue(identifier, out upgradable);
        }

        private bool CheckIdentifierIsNull(TId identifier)
        {
            if (identifier.NotNull()) return false;
            
            Debug.LogWarning(IdentifierIsNull);
            return true;
        }

        #endregion
    }
}
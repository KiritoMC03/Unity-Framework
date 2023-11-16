using General.Mediator;

namespace GameKit.UpgradeModule
{
    public interface IUpgradeSystem<in T> : ISingleComponent
    {
        /// <summary>
        /// Register object to list.
        /// </summary>
        /// <param name="upgradable">Target object</param>
        /// <param name="identifier">Unique key for object.</param>
        /// <param name="setMode"></param>
        /// <typeparam name="T">Identifier type</typeparam>
        /// <returns>Result of try.</returns>
        public AddUpgradableResult Add(T identifier, IUpgradable upgradable, SetMode setMode = SetMode.None);

        /// <summary>
        /// Register object to list.
        /// </summary>
        /// <param name="upgradable">Target object</param>
        /// <param name="identifier">Unique key for object.</param>
        /// <param name="customBaseLevel">Set to levels list by default. If identifier already exists, it is ignored.</param>
        /// <param name="setMode"></param>
        /// <typeparam name="T">Identifier type</typeparam>
        /// <returns>Result of try.</returns>
        public AddUpgradableResult Add(T identifier, IUpgradable upgradable, int customBaseLevel, SetMode setMode = SetMode.None);

        /// <summary>
        /// Try upgrade object.
        /// </summary>
        /// <param name="identifier">Unique key for object.</param>
        /// <returns>Result of try.</returns>
        public bool TryUpgrade(T identifier);

        /// <summary>
        /// Try upgrade object.
        /// </summary>
        /// <param name="identifier">Unique key for object.</param>
        /// <param name="level">Target level for upgrade.</param>
        /// <returns>Result of try.</returns>
        public bool TryUpgrade(T identifier, int level);

        /// <summary>
        /// If the object is registered in the system, upgrades to the registered level.
        /// </summary>
        /// <param name="identifier">Unique key for object.</param>
        /// <returns>Result of try.</returns>
        public bool TryActualizeLevel(T identifier);

        /// <summary>
        /// Find the level by identifier.
        /// </summary>
        /// <param name="identifier">Unique key for object.</param>
        /// <param name="level">Result.</param>
        /// <returns>Result of try.</returns>
        public bool TryGetLevel(T identifier, out int level);
    }
}
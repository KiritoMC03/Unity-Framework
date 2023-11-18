using System.Threading.Tasks;
using UnityEngine;

namespace Framework.Idlers.LevelsSystem
{
    public class CreateLevelFromPrefabStrategy : ICreateLevelStrategy
    {
        #region Fields
        
        protected readonly LevelCreatingByPrefabsPreferences preferences;

        #endregion
        
        #region Constructors

        public CreateLevelFromPrefabStrategy(LevelCreatingByPrefabsPreferences preferences)
        {
            this.preferences = preferences;
        }

        #endregion
        
        #region ICreateLevelStrategy

        public Task<ILevelDefault> CreateNewLevel(int index)
        {
            GameObject prefab = preferences.levelsPrefabs[index].gameObject;
            ILevelDefault result = UnityEngine.Object.Instantiate(prefab, preferences.levelParent).GetComponent<ILevelDefault>();
            return Task.FromResult(result);
        }

        #endregion
    }
}
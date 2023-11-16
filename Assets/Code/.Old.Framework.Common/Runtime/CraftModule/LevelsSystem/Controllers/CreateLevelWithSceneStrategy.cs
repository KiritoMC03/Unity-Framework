using System.Threading.Tasks;
using General.Mediator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameKit.CraftModule.LevelsSystem
{
    public class CreateLevelWithSceneStrategy : ICreateLevelStrategy
    {
        #region ICreateLevelStrategy

        public async Task<ILevelDefault> CreateNewLevel(int index)
        {
            ILevelDefault result = default;
            AsyncOperation operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
            while (!operation.isDone) await Task.Yield();
            await MC.Instance.GetSingleComponentsAsync<CreateLevelWithSceneStrategy, ILevelDefault>(this, SetLevel);
            return result;
            
            void SetLevel(ILevelDefault level) => result = level;
        }

        #endregion
    }
}
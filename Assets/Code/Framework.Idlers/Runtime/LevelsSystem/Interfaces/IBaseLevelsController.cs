using System;
using Framework.Base.Dependencies.Mediator;

namespace Framework.Idlers.LevelsSystem
{
    /// <summary>
    /// Invoke with level, level index and passed levels number
    /// </summary>
    public delegate void LevelCompleteDelegate(ILevelDefault level, int levelIndex, int passedLevelsNumber);
    
    public interface IBaseLevelsController : ISingleComponent
    {
        /// <summary>
        /// Invoke with level index
        /// </summary>
        event Action<int> LevelLoadingStartedCallback;
        /// <summary>
        /// Invoke with level and level index
        /// </summary>
        event Action<ILevelDefault, int> LevelLoadedCallback;
        /// <summary>
        /// Invoke with level, level index and passed levels number
        /// </summary>
        event LevelCompleteDelegate LevelCompletedCallback;
        /// <summary>
        /// Invoke with level and level index
        /// </summary>
        event Action<ILevelDefault, int> LevelPreDestroyedCallback;

        LevelsControllerState State { get; }
        ILevelDefault CurrentLevel { get; }

        void PreInit();
        void Init(ILevelsData levelsData, ICreateLevelStrategy createLevelStrategy);
        void CreateNewLevel(bool preIncreaseIndex = true);
        void CreateNewLevel(int levelIndex);
        void RestartLevel();
    }
}
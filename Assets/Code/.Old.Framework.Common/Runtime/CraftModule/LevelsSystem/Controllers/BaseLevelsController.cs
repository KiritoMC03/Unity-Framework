using System;
using General.Extensions;
using General.Mediator;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameKit.CraftModule.LevelsSystem
{
    public class BaseLevelsController : IBaseLevelsController
    {
        #region Fields

        protected ILevelsData levelsData;
        protected ILevelDefault currentLevel;
        protected ICreateLevelStrategy createLevelStrategy;

        #endregion

        #region Properties

        public virtual ILevelsData LevelsData => levelsData;
        public virtual int TotalLevelsNumber => levelsData.MaxLevelIndex - levelsData.MinLevelIndex + 1;

        #endregion
        
        #region Methods

        protected virtual int CalculateLevelIndex(bool preIncreaseIndex)
        {
            int result = -1;
            result = levelsData.PassedLevelsNumber < TotalLevelsNumber
                ? preIncreaseIndex ? ++levelsData.CurrentLevelIndex : levelsData.CurrentLevelIndex
                : levelsData.CurrentLevelIndex = GetRandomLevelIndex();
            result = levelsData.CurrentLevelIndex = Mathf.Clamp(result, levelsData.MinLevelIndex, levelsData.MaxLevelIndex);
            return result;
        }
        
        protected virtual int GetRandomLevelIndex()
        {
            if (TotalLevelsNumber == 0) 
                Debug.LogError($"Total Levels Number = 0. Please, check Min({levelsData.MinLevelIndex}) and Max({levelsData.MaxLevelIndex}) levels index.");
            if (TotalLevelsNumber < 2) return levelsData.MinLevelIndex;
            int levelIndex = levelsData.CurrentLevelIndex;
            int seed = levelsData.CurrentLevelIndex;
            while (levelIndex == levelsData.CurrentLevelIndex)
            {
                levelIndex = Random.Range(levelsData.MinLevelIndex, levelsData.MaxLevelIndex + 1);
                if (levelIndex == levelsData.CurrentLevelIndex) 
                    Random.InitState(++seed);
            }
            
            return levelIndex;
        }

        protected virtual void HandleLoadedLevel(ILevelDefault level)
        {
            LevelLoadedCallback?.Invoke(level, levelsData.CurrentLevelIndex);
            State = LevelsControllerState.LevelInProgress;
            level.Init();
            level.CompletedCallback += HandleLevelCompleted;
        }

        protected virtual void HandleLevelCompleted()
        {
            levelsData.PassedLevelsNumber++;
            LevelCompletedCallback?.Invoke(currentLevel, levelsData.CurrentLevelIndex, levelsData.PassedLevelsNumber);
            DestroyPrevLevel();
            CreateNewLevel();
        }

        protected virtual void DestroyPrevLevel()
        {
            if (currentLevel.IsNull()) return;
            currentLevel.CompletedCallback -= HandleLevelCompleted;
            LevelPreDestroyedCallback?.Invoke(currentLevel, levelsData.CurrentLevelIndex);
            currentLevel.Destroy();
            State = LevelsControllerState.HasNoLevel;
        }

        #endregion

        #region IBaseLevelsController

        public event Action<int> LevelLoadingStartedCallback;
        public event Action<ILevelDefault, int> LevelLoadedCallback;
        public event LevelCompleteDelegate LevelCompletedCallback;
        public event Action<ILevelDefault, int> LevelPreDestroyedCallback;
        
        public LevelsControllerState State { get; protected set; } = LevelsControllerState.NotInitialized;
        public ILevelDefault CurrentLevel => currentLevel;

        public virtual void PreInit() => MC.Instance.Add<IBaseLevelsController>(this);
        public virtual void Init(ILevelsData levelsData, ICreateLevelStrategy createLevelStrategy)
        {
            this.levelsData = levelsData;
            this.createLevelStrategy = createLevelStrategy;
            State = LevelsControllerState.HasNoLevel;
            levelsData.CurrentLevelIndex = levelsData.CurrentLevelIndex;
        }

        public virtual async void CreateNewLevel(bool preIncreaseIndex = true)
        {
            State = LevelsControllerState.LevelLoading;
            int levelIndex = CalculateLevelIndex(preIncreaseIndex);
            LevelLoadingStartedCallback?.Invoke(levelsData.CurrentLevelIndex);
            currentLevel = await createLevelStrategy.CreateNewLevel(levelIndex);
            HandleLoadedLevel(currentLevel);
        }

        public virtual async void CreateNewLevel(int index)
        {
            index = Mathf.Clamp(index, levelsData.MinLevelIndex, levelsData.MaxLevelIndex);
            levelsData.CurrentLevelIndex = index;   
            State = LevelsControllerState.LevelLoading;
            LevelLoadingStartedCallback?.Invoke(levelsData.CurrentLevelIndex);
            currentLevel = await createLevelStrategy.CreateNewLevel(index);
            HandleLoadedLevel(currentLevel);
        }
        
        public virtual async  void RestartLevel()
        {
            DestroyPrevLevel();
            State = LevelsControllerState.LevelLoading;
            LevelLoadingStartedCallback?.Invoke(levelsData.CurrentLevelIndex);
            currentLevel = await createLevelStrategy.CreateNewLevel(levelsData.CurrentLevelIndex);
            HandleLoadedLevel(currentLevel);
        }
        
        #endregion
    }
}
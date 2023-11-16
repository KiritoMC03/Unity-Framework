using System;
using System.Collections.Generic;
using General;
using General.Extensions;
using General.Mediator;
using UnityEngine;

namespace GameKit.TutorialModule
{
    public class TutorialController : ITutorialController
    {
        #region Fields

        protected TutorialDB dataBase;
        protected SerializedInterfacesList<ITutorialPart> parts;
        protected Dictionary<Type, ITutorialDataStorage> datasDictionary;
        protected readonly IReadOnlyList<int> checkpoints;

        #endregion

        #region Properties

        public virtual ITutorialPart CurrentPart { get; protected set; }

        #endregion

        #region Constructors

        /// <param name="data">Dictionary, where Key - type of current data, Value - data.</param>
        public TutorialController(SerializedInterfacesList<ITutorialPart> parts, 
            Dictionary<Type, ITutorialDataStorage> data,
            IReadOnlyList<int> checkpoints = default)
        {
            this.parts = parts;
            this.datasDictionary = data;
            this.checkpoints = checkpoints;
        }
        
        #endregion

        #region ITutorialController

        public event Action StartedCallback;
        public event Action CompletedCallback;
        public event Action<ITutorialPart> PartStartedCallback;
        public event Action<ITutorialPart> PartCompletedCallback;
        
        public virtual void Init()
        {
            if (MC.Instance.GetSingleComponent(this, out dataBase))
                ApplyCheckpoint(ref dataBase.tutorialPartIndex);
            for (int i = 0; i < parts.Count; i++) InitPart(parts.GetAt(i));
        }

        public virtual void Run()
        {
            InvokeStartedCallback();
            if (parts.Count < 1 || dataBase.tutorialPartIndex > parts.Count - 1)
            {
                InvokeCompletedCallback();
                return;
            }
            CurrentPart = parts.GetAt(dataBase.tutorialPartIndex);
            RunCurrentTutorialPart();
        }
        
        #endregion

        #region Main Methods
        
        protected virtual void InitPart(ITutorialPart part)
        {
            part.TryPrepare();
        }

        protected virtual void ApplyCheckpoint(ref int tutorialIndex)
        {
            if (checkpoints.IsNull()) return;
            for (int i = checkpoints.Count - 1; i >= 0; i--)
            {
                int current = checkpoints[i];
                if (current > tutorialIndex) continue;
                tutorialIndex = current;
                return;
            }
        }

        protected virtual void HandleAllPartsCompleted()
        {
            for (int i = 0; i < parts.Count; i++) parts.GetAt(i).AfterCompleteAllParts();
            InvokeCompletedCallback();
        }
        
        protected virtual void RunCurrentTutorialPart()
        {
            InsertDataToPart(CurrentPart);
            if (CurrentPart.TryStart())
            {
                InvokePartStartedCallback(CurrentPart);
                CurrentPart.CompletedCallback += HandlePartCompleted;
            }
            else
            {
                LogFailedToStartCurrentPart();
                HandlePartCompleted();
            }
        }

        protected virtual void HandlePartCompleted()
        {
            CurrentPart.CompletedCallback -= HandlePartCompleted;
            InvokePartCompletedCallback(CurrentPart);
            if (parts.ContainsIndex(++dataBase.tutorialPartIndex))
            {
                CurrentPart = parts.GetAt(dataBase.tutorialPartIndex);
                RunCurrentTutorialPart();
            }
            else HandleAllPartsCompleted();
        }
        
        #endregion

        #region Utils Methods

        protected virtual void InsertDataToPart(ITutorialPart part)
        {
            IReadOnlyList<Type> neededDataTypes = part.NeededTutorialDataStorageTypes;
            for (int i = 0; i < neededDataTypes.Count; i++)
            {
                if (datasDictionary.TryGetValue(neededDataTypes[i], out ITutorialDataStorage dataStorage))
                    part.TryInsertData(dataStorage);
                else DontContainsDataType(nameof(datasDictionary), neededDataTypes[i]);
            }
        }

        protected virtual void InvokeCompletedCallback() => CompletedCallback?.Invoke();
        protected virtual void InvokeStartedCallback() => StartedCallback?.Invoke();
        protected virtual void InvokePartStartedCallback(ITutorialPart tutorialPart) =>
            PartStartedCallback?.Invoke(tutorialPart);
        protected virtual void InvokePartCompletedCallback(ITutorialPart tutorialPart) =>
            PartCompletedCallback?.Invoke(tutorialPart);

        protected virtual void DontContainsDataType(string containerName, Type type)
        {
            Debug.LogWarning($"{containerName} doesn`t contains data of type \"{type}\"");
        }

        protected virtual void LogFailedToStartCurrentPart()
        {
            Debug.LogWarning($"Tutorial part at index {dataBase.tutorialPartIndex} start failed. ({CurrentPart})");
        }

        #endregion
    }
}
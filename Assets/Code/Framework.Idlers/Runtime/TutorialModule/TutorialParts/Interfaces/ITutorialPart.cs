using System;
using System.Collections.Generic;

namespace Framework.Idlers.TutorialModule
{
    public interface ITutorialPart
    {
        public event Action CompletedCallback;

        public IReadOnlyList<Type> NeededTutorialDataStorageTypes { get; }
        
        public bool TryPrepare();
        public bool TryStart();
        public void Complete();
        public void AfterCompleteAllParts();
        public bool TryInsertData<TData>(TData inputData) where TData : ITutorialDataStorage;
    }
}
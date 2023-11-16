using System;
using General.Mediator;

namespace GameKit.CraftModule.LevelsSystem
{
    public interface ILevelDefault : ISingleComponent
    {
        #region Events

        event Action CompletedCallback;

        #endregion

        #region Methods

        void Init();
        void Destroy();

        #endregion
    }
}
using System;
using Framework.Base.Dependencies.Mediator;

namespace Framework.Idlers.LevelsSystem
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
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Idlers.UI
{
    public abstract class ScreenControllerBase
    {
        #region Fields

        protected List<object> createScreenCommands = new List<object>();

        #endregion
        
        #region Properties

        public virtual IScreen CurrentScreen { get; protected set; }
        public abstract Transform Parent { get; }

        #endregion
        
        #region Methods

        public virtual void CreateScreen<T>()
            where T: IScreen
        {
            foreach (object command in createScreenCommands)
            {
                if (command is not ICreateScreenCommand<T> realCommand)
                    continue;

                CurrentScreen = realCommand.Create();
                return;
            }
        }

        public virtual void TryDestroyCurrentScreen()
        {
            if (CurrentScreen == null) return;
            Object.Destroy(CurrentScreen.GameObject);
            CurrentScreen = null;
        }

        protected virtual void SetCreateScreenCommands(IEnumerable<object> commands)
        {
            createScreenCommands.AddRange(commands);
        }

        #endregion
    }
}
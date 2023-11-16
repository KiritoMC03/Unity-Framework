using System;
using System.Collections.Generic;

namespace GameKit.CraftModule.Interfaces
{
    public interface IReceiver<TObject, out TMatcher>
    {
        #region Events

        public event Action ReceivedCallback;
        public event Action<TObject> ItemReceivedCallback;

        #endregion
        
        #region Properties

        public bool CanReceive { get; }

        #endregion
        
        #region Methods

        public IEnumerable<TMatcher> GetAcceptedMatchers();

        public bool TryReceive(TObject target);

        #endregion
    }
}
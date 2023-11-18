using System.Collections.Generic;

namespace Framework.Idlers.GameTicks
{
    public class GameTicksManager
    {
        #region Fields

        private List<IGameTickHandler> gameTickHandlers = new List<IGameTickHandler>(10);
        private List<IFixedGameTickHandler> fixedGameTickHandlers = new List<IFixedGameTickHandler>(10);
        private List<ILongGameTickHandler> longGameTickHandlers = new List<ILongGameTickHandler>(10);
        private long ticksNumber;

        private const int TicksNumberInLongTick = 10;

        #endregion

        #region Methods

        public void AddTickHandler(IGameTickHandler handler) => gameTickHandlers.Add(handler);
        public void AddFixedTickHandler(IFixedGameTickHandler handler) => fixedGameTickHandlers.Add(handler);
        public void AddLongTickHandler(ILongGameTickHandler handler) => longGameTickHandlers.Add(handler);
        
        public void SimulateTick()
        {
            ticksNumber++;
            foreach (IGameTickHandler item in gameTickHandlers)
                item.HandleGameTick();
            if (ticksNumber % TicksNumberInLongTick == 0)
                foreach (ILongGameTickHandler item in longGameTickHandlers)
                    item.HandleLongGameTick();
        }

        public void SimulateFixedTick()
        {
            foreach (IFixedGameTickHandler item in fixedGameTickHandlers)
                item.HandleFixedGameTick();
        }

        #endregion
    }

    public interface IGameTickHandler
    {
        void HandleGameTick();
    }

    public interface IFixedGameTickHandler
    {
        void HandleFixedGameTick();
    }
    
    public interface ILongGameTickHandler
    {
        void HandleLongGameTick();
    }
}
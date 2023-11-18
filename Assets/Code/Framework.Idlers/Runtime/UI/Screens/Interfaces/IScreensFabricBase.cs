namespace Framework.Idlers.UI
{
    public interface IScreensFabricBase
    {
        #region Properties

        public bool HasScreen { get; }
        public ScreenBase CurrentScreen { get; }

        #endregion
        
        #region Methods

        public ScreenBase ShowScreen<T>(bool destroyPrevScreen = true) where T : ScreenBase;
        public void TryDestroyCurrentScreen();

        #endregion
    }
}
namespace Framework.Base.SaveLoad
{
    public interface ISaveLoadSystem
    {
        #region Methods

        bool TrySave<T>(ref T data) where T : struct;
        bool TryLoad<T>(ref T data) where T : struct;

        #endregion
    }
}
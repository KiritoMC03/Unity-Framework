namespace General.SaveLoad
{
    /// <summary>
    /// Using in save load system.
    /// </summary>
    public interface ISaveLoadCallbackReceiver
    {
        public void OnBeforeSerialize();
        public void OnAfterDeserialize();
    }
}
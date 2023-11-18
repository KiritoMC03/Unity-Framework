namespace Framework.Idlers.LevelsSystem
{
    public interface ILevelsData
    {
        public int MinLevelIndex { get; set; }
        public int MaxLevelIndex { get; set; }
        public int CurrentLevelIndex { get; set; }
        public int PassedLevelsNumber { get; set; }
    }
}
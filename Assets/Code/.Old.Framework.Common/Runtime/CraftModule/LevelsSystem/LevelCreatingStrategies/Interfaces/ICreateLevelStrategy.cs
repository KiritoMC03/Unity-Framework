using System.Threading.Tasks;

namespace GameKit.CraftModule.LevelsSystem
{
    public interface ICreateLevelStrategy
    {
        Task<ILevelDefault> CreateNewLevel(int index);
    }
}
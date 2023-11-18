using System.Threading.Tasks;

namespace Framework.Idlers.LevelsSystem
{
    public interface ICreateLevelStrategy
    {
        Task<ILevelDefault> CreateNewLevel(int index);
    }
}
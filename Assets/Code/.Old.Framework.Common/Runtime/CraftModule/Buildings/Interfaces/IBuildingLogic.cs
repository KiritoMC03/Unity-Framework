using System.Threading.Tasks;
using General.Mediator;
using UnityEngine;

namespace GameKit.CraftModule.Buildings
{
    public interface IBuildingLogic : IComponents
    {
        GameObject GameObject { get; }
        Transform Transform { get; }
        
        void Init(int id);
        Task PlayBuiltVisualization(bool doImmediately = false);
    }
}
using System.Threading.Tasks;
using Framework.Base.Dependencies.Mediator;
using UnityEngine;

namespace Framework.Idlers.Buildings
{
    public interface IBuildingLogic : IComponents
    {
        GameObject GameObject { get; }
        Transform Transform { get; }
        
        void Init(int id);
        Task PlayBuiltVisualization(bool doImmediately = false);
    }
}
using System;
using System.Collections.Generic;
using Framework.Base.Dependencies.Mediator;
using Framework.Idlers.Resource;

namespace Framework.Idlers.Buildings
{
    public interface IBuilding : IComponents
    {
        event Action<IBuilding> BuiltCallback;
        
        void Init<TDic>(Dictionary<int, TDic> buildingsData)
            where TDic : Dictionary<ResourceType, int>, new();
        void Build();
        
        IBuildingLogic BuildingLogic { get; }
        IBuildZone BuildZone { get; }
    }
}
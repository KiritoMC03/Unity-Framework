using System;
using System.Collections.Generic;
using GameKit.CraftModule.Resource;
using General.Mediator;

namespace GameKit.CraftModule.Buildings
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
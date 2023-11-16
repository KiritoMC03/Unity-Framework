using System;
using System.Collections.Generic;
using GameKit.CraftModule.Resource;
using GameKit.General.Extensions;
using General;
using General.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace GameKit.CraftModule.Buildings
{
    public class BuildingController : MonoBehaviour, IBuilding
    {
        #region Events
        
        public UnityEvent<IBuilding> UnityBuiltCallback;
        public event Action<IBuilding> BuiltCallback;

        #endregion
        
        #region Fields

        [SerializeField]
        protected ObservableDictionary<ResourceType, int> defaultResourcesForBuild;

        [SerializeField]
        protected InterfaceItem<IBuildingLogic> buildingLogicComponent;

        [SerializeField]
        protected InterfaceItem<IBuildZone> buildZonePrefab;

        [SerializeField]
        protected BuildingControllerData data;

        protected IBuildZone buildZone;
        protected IBuildingLogic buildingLogic;
        
        #endregion

        #region Properties

        public virtual IBuildingLogic BuildingLogic => buildingLogic;
        public virtual IBuildZone BuildZone => buildZone;

        #endregion

        #region Unity lifecycle

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            
            Matrix4x4 matrix = Matrix4x4.TRS(transform.position + data.buildZoneOffset.Rotate(transform.rotation), 
                Quaternion.Euler(data.zoneRotation.Rotate(transform.rotation)), 
                data.buildZoneScale.Multiply(transform.lossyScale));
            Matrix4x4 tempMatrix = Gizmos.matrix;
            Gizmos.matrix = matrix;
            Gizmos.DrawCube(Vector3.zero, new Vector3(1f, 0.01f, 1f));
            Gizmos.matrix = tempMatrix;
        }
#endif

        #endregion
        
        #region Methods

        public virtual int GetID() => GetInstanceID();

        protected virtual void LoadResourcesForBuild<T, TOut>(Dictionary<int, TOut> buildingsData, out TOut resourcesForBuild)
            where TOut : Dictionary<ResourceType, int>, new()
        {
            int id = GetID();
            if (buildingsData.ContainsKey(id))
            {
                resourcesForBuild = buildingsData[id];
                return;
            }
            
            TOut tempDictionary = new TOut();
            foreach (KeyValuePair<ResourceType, int> i in defaultResourcesForBuild) tempDictionary.Add(i.Key, i.Value);
            buildingsData.Add(id, tempDictionary);
            resourcesForBuild = tempDictionary;
        }
        
        protected virtual void InitBuildingZone(Dictionary<ResourceType, int> resourcesForBuild)
        {
            if (data.useCollisionResolver) data.collisionResolver.MakeTransparent();
            buildZone = Instantiate(buildZonePrefab.Component, transform).GetComponent<IBuildZone>();
            buildZone.Transform.localPosition = data.buildZoneOffset;
            buildZone.Transform.localRotation = Quaternion.Euler(data.zoneRotation);
            buildZone.Transform.localScale = data.buildZoneScale;
            buildZone.AllResourcesReceivedCallback += Build;
            buildZone.Init(resourcesForBuild);
            buildZone.SwitchActivity(true);
        }

        protected virtual void InvokeBuiltCallback()
        {
            BuiltCallback?.Invoke(this);
            UnityBuiltCallback?.Invoke(this);
        }
        
        #endregion
        
        #region IBuilding

        public virtual void Init<T>(Dictionary<int, T> buildingsData)
            where T : Dictionary<ResourceType, int>, new()
        {
            buildingLogic = buildingLogicComponent as IBuildingLogic;
            LoadResourcesForBuild<Dictionary<int, T>, T>(buildingsData, out T resourcesForBuild);
            if (resourcesForBuild.Count < 1) Build();
            else InitBuildingZone(resourcesForBuild);
        }

        public virtual async void Build()
        {
            if (buildingLogic.NotNull())
                await buildingLogic.PlayBuiltVisualization(buildZone.IsNull());

            if (data.useCollisionResolver) data.collisionResolver.Destroy();
            buildZone?.Destroy();
            buildingLogic?.Init(GetID());
            InvokeBuiltCallback();
        }

        #endregion
    }
}
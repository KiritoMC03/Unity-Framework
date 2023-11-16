using System;
using GameKit.CraftModule.Resource;
using General;
using General.Extensions;
using UnityEngine;

namespace GameKit.TutorialModule
{
    [Serializable]
    public class InteractWithResourceReceiverTutorialPartData : ITutorialDataStorage
    {
        private const string ListOfResourcesToSendTooltip =
            "A list of resources that need to be submitted to complete the tutorial step.";
        
        [InterfaceChecker(typeof(IResourceReceiver))]
        public Component resourceReceiverComponent;

        [SerializeField] [Tooltip(ListOfResourcesToSendTooltip)]
        public SerializedDictionary<ResourceType, int> listOfResourcesToReceive;

        [HideInInspector]
        public IResourceReceiver resourceReceiver;

        public void Validate()
        {
            if (resourceReceiverComponent.LogIfNull()) return;
            resourceReceiver = resourceReceiverComponent as IResourceReceiver;
        }
    }
}
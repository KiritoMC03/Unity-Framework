using System;
using Framework.Base;
using Framework.Base.Collections;
using Framework.Base.Extensions;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.TutorialModule
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
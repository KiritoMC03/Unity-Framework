using System;
using Framework.Base;
using Framework.Base.Collections;
using Framework.Base.Extensions;
using Framework.Idlers.Resource;
using UnityEngine;

namespace Framework.Idlers.TutorialModule
{
    [Serializable]
    public class InteractWithResourceSenderTutorialPartData : ITutorialDataStorage
    {
        private const string ListOfResourcesToSendTooltip =
            "A list of resources that need to be submitted to complete the tutorial step.";
        
        [InterfaceChecker(typeof(IResourceSender))]
        public Component resourceSenderComponent;

        [SerializeField] [Tooltip(ListOfResourcesToSendTooltip)]
        public SerializedDictionary<ResourceType, int> listOfResourcesToSend;

        [HideInInspector]
        public IResourceSender resourceSender;

        public void Validate()
        {
            if (resourceSenderComponent.LogIfNull()) return;
            resourceSender = resourceSenderComponent as IResourceSender;
        }
    }
}
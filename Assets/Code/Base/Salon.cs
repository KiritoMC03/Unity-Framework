using System;
using Framework.Base.ComponentModel;
using Framework.Idlers.ScriptableObject;
using UnityEngine;
using Component = Framework.Base.ComponentModel.Component;

namespace Code
{
    [Serializable] [AutoContainer]
    public class Salon : Component
    {
        [SerializeField]
        private SalonData data;

        public void LogSome()
        {
            Debug.Log($"Some: {data.IdentifierRef.GetIdentifier()}, pos: {data.CarContainer.Component.Transform.position}");
        }
    }

    [Serializable]
    public class SalonData
    {
        [field: SerializeField]
        public UniqueScriptableObjectIdentifierRef IdentifierRef { get; private set; }
        
        [field: SerializeField]
        public ComponentContainer<Car> CarContainer { get; private set; }
    }
}
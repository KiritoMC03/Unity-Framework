using System;
using Framework.Base.ComponentModel;
using UnityEngine;
using Component = Framework.Base.ComponentModel.Component;

namespace Code
{
    [Serializable] [AutoContainer]
    public class Car : Component
    {
        public Transform Transform => source.transform;
    }
}
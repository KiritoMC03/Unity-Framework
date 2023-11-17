using System;
using UnityEngine;

namespace Framework.Base
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InterfaceCheckerAttribute : PropertyAttribute
    {
        public readonly Type[] Types;

        public InterfaceCheckerAttribute(params Type[] types) =>
            Types = types ?? throw new ArgumentNullException(typeof(Type[]).ToString());
    }
}
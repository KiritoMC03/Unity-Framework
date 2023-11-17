using System;
using UnityEngine;

namespace Framework.Base
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyOnPlayAttribute : PropertyAttribute
    {
    }
}
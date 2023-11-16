using System;
using UnityEngine;

namespace General
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HideIfAttribute : PropertyAttribute
    {
        public readonly string Toggle;

        public HideIfAttribute(string toggle) => Toggle = toggle;
    }
}
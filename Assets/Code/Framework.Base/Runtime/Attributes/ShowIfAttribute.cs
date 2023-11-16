using System;
using UnityEngine;

namespace General
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string Toggle;

        public ShowIfAttribute(string toggle) => Toggle = toggle;
    }
}
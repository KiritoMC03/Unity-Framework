using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyOnPlayAttribute : PropertyAttribute
    {
    }
}
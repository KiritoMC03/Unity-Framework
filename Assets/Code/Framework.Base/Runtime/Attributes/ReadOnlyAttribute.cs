using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Base
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}
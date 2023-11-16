using System;
using UnityEngine;
using GameKit.CraftModule.Resource;

namespace GameKit.CraftModule.ResourceTransferHelper
{
    [Serializable]
    public class BackpackPreferences
    {
        public Transform[] anchors;
        public PaymentConfig paymentConfig;
    }
}
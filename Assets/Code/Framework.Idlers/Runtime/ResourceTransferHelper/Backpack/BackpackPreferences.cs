using System;
using UnityEngine;

namespace Framework.Idlers.ResourceTransferHelper
{
    [Serializable]
    public class BackpackPreferences
    {
        public Transform[] anchors;
        public PaymentConfig paymentConfig;
    }
}
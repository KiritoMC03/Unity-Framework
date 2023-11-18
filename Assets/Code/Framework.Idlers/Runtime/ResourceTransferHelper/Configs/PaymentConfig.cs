using UnityEngine;

namespace Framework.Idlers.ResourceTransferHelper
{
    [CreateAssetMenu(fileName = "PaymentConfig", menuName = "Framework/Idlers/New Payment Config", order = 101)]
    public class PaymentConfig : UnityEngine.ScriptableObject
    {
        #region Fields

        [SerializeField]
        private float defaultCooldownTime;

        [SerializeField]
        private float stepProgression;

        [SerializeField]
        private float minCooldownTime;

        [SerializeField]
        private float resourceJumpDuration;

        #endregion

        #region Properties

        public float DefaultCooldownTime => defaultCooldownTime;
        public float StepProgression => stepProgression;
        public float MinCooldownTime => minCooldownTime;
        public float ResourceJumpDuration => resourceJumpDuration;

        #endregion
    }
}
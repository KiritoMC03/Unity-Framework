using UnityEngine;

namespace GameKit.CraftModule.Resource
{
    [CreateAssetMenu(fileName = "PaymentConfig", menuName = "Game/New Payment Config", order = 0)]
    public class PaymentConfig : ScriptableObject
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
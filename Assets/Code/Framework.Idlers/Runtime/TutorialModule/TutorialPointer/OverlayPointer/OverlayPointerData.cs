using Framework.Base;
using UnityEngine;

namespace Framework.Idlers.TutorialModule
{
    [CreateAssetMenu(fileName = "OverlayPointerConfig", menuName = "Framework/Idlers/Tutorial/New overlay pointer config", order = 101)]
    public class OverlayPointerData : UnityEngine.ScriptableObject
    {
        public Vector2 startedPosition;
        public PointInfo[] points;
        public bool isCycled;
        [ShowIf("isCycled")]
        public bool smoothReturnToStartedPosition;
        [ShowIf("isCycled")] 
        public float smoothReturnToStartedPositionDuration;
    }
}
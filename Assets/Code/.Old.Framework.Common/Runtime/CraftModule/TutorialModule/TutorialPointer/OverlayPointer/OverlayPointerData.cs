using General;
using UnityEngine;

namespace GameKit.TutorialModule
{
    [CreateAssetMenu(fileName = "OverlayPointerConfig", menuName = "Game/Tutorial/New overlay pointer config", order = 0)]
    public class OverlayPointerData : ScriptableObject
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
using Framework.Idlers.Utils;
using UnityEngine;

namespace Framework.Idlers.UI
{
    public class SafeArea : MonoBehaviour
    {
        #region Fields

        public InitMode initMode;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            if (initMode == InitMode.OnAwake) Init();
        }

        private void Start()
        {
            if (initMode == InitMode.OnStart) Init();
        }

        #endregion

        #region Methods
        
        public void Init()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            Rect safeArea = Screen.safeArea;
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= screenWidth;
            anchorMin.y /= screenHeight;
            anchorMax.x /= screenWidth;
            anchorMax.y /= screenHeight;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }

        #endregion
    }
}
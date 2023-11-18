using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Framework.Base.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Idlers.TutorialModule
{
    public class OverlayPointer : MonoBehaviour
    {
        #region Fields

        [SerializeField] 
        private Image overlayPointerImage;

        [SerializeField]
        private Sprite defaultSprite;
        
        [SerializeField]
        private Sprite clickedSprite;
        
        private OverlayPointerData pointerData;
        private Tweener currentTweener;
        private Tweener returnToStartTweener;
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken cancellationToken;
        
        #endregion

        #region Unity lifecycle

        private void OnDisable()
        {
            cancelTokenSource.Cancel();
        }

        private void OnDestroy()
        {
            cancelTokenSource.Cancel();
        }

        private void OnEnable()
        {
            InitCancellationToken();
        }

        #endregion
        
        #region Methods

        private void InitCancellationToken()
        {
            cancelTokenSource = new CancellationTokenSource();
            cancellationToken = cancelTokenSource.Token;
        }
        
        public void Init(OverlayPointerData data)
        {
            pointerData = data;
            InitCancellationToken();
        }

        public async void StartHesitation()
        {
            if (cancellationToken.IsCancellationRequested) return;
            
            Show();
            if (pointerData.smoothReturnToStartedPosition) await SmoothReturnToStartedPosition();
            else transform.localPosition = pointerData.startedPosition;

            currentTweener?.Kill();
            for (int i = 0; i < pointerData.points.Length; i++)
            {
                PointInfo currentPoint = pointerData.points[i];
                await GetPointerClickState(i);
                if (cancellationToken.IsCancellationRequested) return;
                currentTweener = transform.DOLocalMove(currentPoint.position, currentPoint.movementDuration);
                currentTweener.SetLink(gameObject);
                while (currentTweener.active && !currentTweener.IsComplete())
                {
                    if (cancellationToken.IsCancellationRequested) return;
                    await Task.Yield();
                }
            }

            if (pointerData.isCycled) StartHesitation();
            else Hide();
        }

        private Task SmoothReturnToStartedPosition()
        {
            returnToStartTweener?.Kill();
            returnToStartTweener = transform.DOLocalMove(pointerData.startedPosition, pointerData.smoothReturnToStartedPositionDuration);
            returnToStartTweener.SetLink(gameObject);
            int duration = Mathf.RoundToInt(pointerData.smoothReturnToStartedPositionDuration * 1000f);
            return Task.Delay(duration);
        }
        
        private async Task GetPointerClickState(int pointIndex)
        {
            ClickMode clickMode = pointerData.points[pointIndex].clickMode;
            float clickDuration = pointerData.points[pointIndex].onceClickDuration;
            
            switch (clickMode)
            {
                case ClickMode.None: 
                    SetDefaultPointerSprite();
                    break;
                case ClickMode.ClickOnPoint:
                    await PlayOnceClick(clickDuration);
                    break;
                case ClickMode.AlwaysPressed:
                    SetClickedPointerSprite();
                    break;
            }
        }

        private async Task PlayOnceClick(float clickDuration)
        {
            SetClickedPointerSprite();
            await Task.Delay((int)(clickDuration * 1000f));
            if (cancellationToken.IsCancellationRequested) return;
            SetDefaultPointerSprite();
        }

        private void SetClickedPointerSprite() => overlayPointerImage.sprite = clickedSprite;
        private void SetDefaultPointerSprite() => overlayPointerImage.sprite = defaultSprite;
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        public void Destroy()
        {
            currentTweener?.Kill();
            returnToStartTweener?.Kill();
            gameObject.DestroyNotNull();
        }

        #endregion
    }
}
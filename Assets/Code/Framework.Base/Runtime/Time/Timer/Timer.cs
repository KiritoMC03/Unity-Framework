using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Framework.Base
{
    public class Timer
    {
        #region Events

        public event Action StartedCallback;
        public event Action<TimerState> UpdatedCallback;
        public event Action CompletedCallback;

        #endregion

        #region Fields

        private CancellationTokenSource rootCancellationTokenSource;

        #endregion

        #region Properties

        public TimerCalculateType CalculateType { get; set; } = TimerCalculateType.Regressive;

        #endregion

        #region Methods

        public async UniTask Run(float time, CancellationToken cancellationToken)
        {
            rootCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            await TimerLifecycle(time, rootCancellationTokenSource.Token);
        }

        /// <summary>
        /// Reset all callbacks and stops timer
        /// </summary>
        public void Reset()
        {
            rootCancellationTokenSource.Cancel();
            StartedCallback = default;
            UpdatedCallback = default;
            CompletedCallback = default;
        }

        #endregion

        #region Async

        protected virtual async UniTask TimerLifecycle(float time, CancellationToken cancellationToken)
        {
            StartedCallback?.Invoke();
            float currentTime = CalculateType == TimerCalculateType.Progressive ? 0 : time;
            while (true)
            {
                currentTime += Time.deltaTime * (int)CalculateType;
                UpdatedCallback?.Invoke(new TimerState(currentTime));

                if ((CalculateType == TimerCalculateType.Progressive && currentTime >= time) ||
                    (CalculateType == TimerCalculateType.Regressive && currentTime < 0))
                    break;

                await UniTask.Yield();
                if (cancellationToken.IsCancellationRequested)
                    return;
            }

            currentTime = CalculateType == TimerCalculateType.Progressive ? time : 0;
            UpdatedCallback?.Invoke(new TimerState(currentTime));
            CompletedCallback?.Invoke();
        }

        #endregion
    }
}
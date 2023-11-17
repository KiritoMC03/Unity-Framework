using Framework.Base.Extensions;
using UnityEngine;

namespace Framework.Base
{
    public readonly struct TimerState
    {
        public float Time { get; }
        public int Seconds { get; }
        public int Milliseconds { get; }

        public TimerState(float time)
        {
            Time = time;
            Seconds = Mathf.RoundToInt(time);
            Milliseconds = Mathf.RoundToInt((time - (int)time) * 1000);
        }

        /// <param name="separator">separator between digits</param>
        /// <param name="minDigitsNumber">min digits number in timer (2 = 00:00, 3 = 0:00:00)</param>
        public string ToString(char separator = ':', int minDigitsNumber = 2) =>
            Seconds.AsTimer(separator, minDigitsNumber);
    }
}
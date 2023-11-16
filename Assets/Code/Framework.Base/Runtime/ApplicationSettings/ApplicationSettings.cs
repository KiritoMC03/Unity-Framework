using UnityEngine;

namespace GameKit.General.ApplicationSettings
{
    public class ApplicationSettings : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private FrameRate targetFrameRate = FrameRate._60;

        [SerializeField]
        private SleepTimeoutPreference sleepTimeoutPreference = SleepTimeoutPreference.NeverSleep;

        [SerializeField]
        [Min(0)]
        [Header("In seconds.")]
        private int customSleepTimeoutPreference = 300;

        #endregion

        #region Unity lifecycle

        private void Awake()
        {
            SetFrameRate(targetFrameRate);
            SetSleepTimeout(sleepTimeoutPreference);
        }

        #endregion

        #region Methods

        private void SetSleepTimeout(SleepTimeoutPreference sleepTimeout)
        {
            switch (sleepTimeout)
            {
                case SleepTimeoutPreference.NeverSleep:
                    Screen.sleepTimeout = SleepTimeout.NeverSleep;
                    break;
                case SleepTimeoutPreference.SystemSetting:
                    Screen.sleepTimeout = SleepTimeout.SystemSetting;
                    break;
                case SleepTimeoutPreference.Custom:
                    Screen.sleepTimeout = customSleepTimeoutPreference;
                    break;
                default:
                    Screen.sleepTimeout = SleepTimeout.SystemSetting;
                    break;
            }
        }

        private void SetFrameRate(FrameRate rate)
        {
            Application.targetFrameRate = (int)rate;
        }

        #endregion
    }
}
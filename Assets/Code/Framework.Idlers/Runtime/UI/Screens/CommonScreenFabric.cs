using System.Collections.Generic;
using Framework.Base.Extensions;
using Framework.Idlers.UI.Extensions;
using UnityEngine;

namespace Framework.Idlers.UI
{
    public class CommonScreenFabric : IScreensFabricBase
    {
        #region Fields

        protected readonly ICommonScreenFabricData data;
        protected readonly Transform screensParent;
        protected HashSet<ScreenBase> reusableScreens;

        #endregion
        
        #region Constructors

        public CommonScreenFabric(ICommonScreenFabricData data, Transform screensParent)
        {
            this.data = data;
            this.screensParent = screensParent;
            reusableScreens = new HashSet<ScreenBase>();
        }

        #endregion

        #region Methods

        protected virtual bool TryGetReusable<T>(out ScreenBase result)
            where T : ScreenBase
        {
            foreach (ScreenBase reusableScreen in reusableScreens)
            {
                if (!(reusableScreen is T)) continue;
                result = reusableScreen;
                reusableScreens.Remove(reusableScreen);
                return true;
            }

            result = default;
            return false;
        }

        protected virtual ScreenBase GetScreen<T>()
            where T : ScreenBase
        {
            if (TryGetReusable<T>(out ScreenBase result))
            {
                result.Show();
                return result;
            }
            return data.ScreensPrefabs.ShowInitedScreen<T>(screensParent);
        }

        #endregion
        
        #region IScreensFabricBase

        public virtual bool HasScreen => CurrentScreen.NotNull();
        public virtual ScreenBase CurrentScreen { get; protected set; }
        
        public virtual ScreenBase ShowScreen<T>(bool destroyPrevScreen = true) 
            where T : ScreenBase
        {
            if (destroyPrevScreen) TryDestroyCurrentScreen();
            return CurrentScreen = GetScreen<T>();
        }

        public virtual void TryDestroyCurrentScreen()
        {
            if (CurrentScreen.IsNull()) return;
            if (CurrentScreen.ReuseInstance)
            {
                reusableScreens.Add(CurrentScreen);
                CurrentScreen.Hide();
                CurrentScreen = default;
                return;
            }
            
            CurrentScreen.TryDestroyGameObject();
            CurrentScreen = default;
        }

        #endregion
    }
}
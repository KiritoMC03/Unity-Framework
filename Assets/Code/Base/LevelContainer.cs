using System;
using Framework.Base.ComponentModel;

namespace Code
{
    public class LevelContainer : ComponentContainer<Level>
    {
    }

    [Serializable]
    public class Level : Component
    {
        public ComponentContainer<Salon> salonContainer;

        protected override void Construct()
        {
            salonContainer.Component.LogSome();
        }
    }
}
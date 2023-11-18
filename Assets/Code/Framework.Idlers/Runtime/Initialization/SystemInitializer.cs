using System.Collections.Generic;

namespace Framework.Idlers.Initialization
{
    public class SystemInitializer
    {
        private readonly List<object> systems;

        public SystemInitializer(params object[] systems)
        {
            this.systems = new List<object>(systems);
        }

        public void AddSystem(object system) => systems.Add(system);

        public void PreInitAll()
        {
            foreach (object system in systems)
                if (system is IPreInitSystem s)
                    s.PreInit();
        }

        public void InitAll()
        {
            foreach (object system in systems)
                if (system is IInitSystem s)
                    s.Init();
        }
    }
}
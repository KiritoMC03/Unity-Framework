namespace General.Mediator
{
    internal struct ComponentData
    {
        #region Fields

        public readonly int Index;
        public bool IsExist;
        public readonly ComponentType ComponentType;

        #endregion

        public ComponentData(int index, bool isExist, ComponentType componentType)
        {
            Index = index;
            IsExist = isExist;
            ComponentType = componentType;
        }
    }
}

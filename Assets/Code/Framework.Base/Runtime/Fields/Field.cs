namespace Framework.Base
{
    public static class Field
    {
        public static T Extract<T>(ref T field)
        {
            T result = field;
            field = default;
            return result;
        }
    }
}
namespace GameKit.Conversion
{
    public interface IConvertible<TMatcher>
    {
        public TMatcher Matcher { get; }
    }
}
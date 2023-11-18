namespace Framework.Idlers.UI
{
    public interface ICreateScreenCommand<out T> 
        where T : IScreen
    {
        T Create();
    }
}
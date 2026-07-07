namespace Puya.Service
{
    public interface IService
    {
        string Name { get; }
        IServiceAction GetAction(string name);
        IServiceAction this[string action] { get; }
    }
}

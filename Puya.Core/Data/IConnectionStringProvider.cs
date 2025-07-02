namespace Puya.Data
{
    public interface IConnectionStringProvider
    {
        int Count { get; }
        void SetCurrent(string name);
        string GetCurrent();
        string GetConnectionString();
        string GetConnectionString(string name);
        void SetConnectionString(string constr);
        void SetConnectionString(string name, string constr);
        void Remove(string name);
    }
}

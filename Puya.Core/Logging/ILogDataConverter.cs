namespace Puya.Logging
{
    public interface ILogDataConverter
    {
        object Deserialize(string data);
        string Serialize(object data);
    }
}

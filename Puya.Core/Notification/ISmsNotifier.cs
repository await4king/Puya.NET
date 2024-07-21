namespace Puya.Notification
{
    public interface ISmsNotifier
    {
        void Notify(string target, string message);
    }
}

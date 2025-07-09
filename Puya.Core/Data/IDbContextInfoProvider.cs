namespace Puya.Data
{
    public interface IDbContextInfoProvider
    {
        void SetContextInfo(string contextInfo);
        string GetContextInfo();
    }
}

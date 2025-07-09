namespace Puya.Data
{
    public class NullDbContextInfoProvider : IDbContextInfoProvider
    {
        public string GetContextInfo()
        {
            return string.Empty;
        }

        public void SetContextInfo(string contextInfo)
        {
        }
    }
}

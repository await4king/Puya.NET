namespace Puya.Data
{
    public class DefaultDbContextInfoProvider : IDbContextInfoProvider
    {
        protected string contextInfo;
        public virtual string GetContextInfo()
        {
            return contextInfo;
        }

        public virtual void SetContextInfo(string contextInfo)
        {
            if (string.IsNullOrEmpty(this.contextInfo))
            {
                this.contextInfo = contextInfo;
            }
        }
        public virtual void SetContextInfo(string contextInfo, bool force)
        {
            if (string.IsNullOrEmpty(this.contextInfo))
            {
                this.contextInfo = contextInfo;
            }
            else
            {
                if (force)
                {
                    this.contextInfo = contextInfo;
                }
            }
        }
    }
}

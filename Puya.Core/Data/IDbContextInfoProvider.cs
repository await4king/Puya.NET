using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Data
{
    public interface IDbContextInfoProvider
    {
        void SetContextInfo(string contextInfo);
        string GetContextInfo();
    }

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

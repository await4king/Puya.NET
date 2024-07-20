using Puya.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Mail
{
    public enum FakeMailType
    {
        None, File, Debug, Console, Trace
    }
    public class FakeDynamicMailManager : IMailManager
    {
        private IMailManager mailer;
        public virtual IMailConfig Config
        {
            get { return mailer.Config; }
            set
            {
                if (mailer != null)
                    mailer.Config = value;
            }
        }
        public virtual IMailManager Instance { get { return mailer; } }
        public FakeDynamicMailManager(string mailerType)
        {
            var type = FakeMailType.None;

            Enum.TryParse<FakeMailType>(mailerType, out type);

            switch (type)
            {
                case FakeMailType.Console: mailer = new FakeConsoleMailManager(); break;
                case FakeMailType.Debug: mailer = new FakeDebugMailManager(); break;
                case FakeMailType.Trace: mailer = new FakeTraceMailManager(); break;
                case FakeMailType.File: mailer = new FakeFileMailManager(); break;
                case FakeMailType.None: mailer = null; break;
                default:
                    throw new Exception("invalid fake mailer type: " + type);
            }
        }
        public virtual bool Send(string to, string subject, string body, bool isHtml = false)
        {
            return Instance?.Send(to, subject, body, isHtml) ?? false;
        }

        public virtual bool Send(string to, string subject, string body, bool isHtml, IEnumerable<string> cc, IEnumerable<string> bcc)
        {
            return Instance?.Send(to, subject, body, isHtml, cc, bcc) ?? false;
        }

        public virtual Task<bool> SendAsync(string to, string subject, string body, bool isHtml = false)
        {
            if (Instance != null)
                return Instance.SendAsync(to, subject, body, isHtml);
            
            return Task.FromResult(false);
        }

        public virtual Task<bool> SendAsync(string to, string subject, string body, bool isHtml, IEnumerable<string> cc, IEnumerable<string> bcc)
        {
            if (Instance != null)
                return Instance.SendAsync(to, subject, body, isHtml, cc, bcc);

            return Task.FromResult(false);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Puya.Mail
{
    public interface IMailManager
    {
        IMailConfig Config { get; set; }
        bool Send(string to, string subject, string body, bool isHtml = false);
        Task<bool> SendAsync(string to, string subject, string body, bool isHtml = false);
        bool Send(string to, string subject, string body, bool isHtml, IEnumerable<string> cc, IEnumerable<string> bcc);
        Task<bool> SendAsync(string to, string subject, string body, bool isHtml, IEnumerable<string> cc, IEnumerable<string> bcc);
    }
}

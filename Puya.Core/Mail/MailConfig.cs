using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Mail
{
    public class MailConfigItem: BaseMailConfig
    {
        public string Name { get; set; }
    }
    public class MailConfig: List<MailConfigItem>
    {
    }
}

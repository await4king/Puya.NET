using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puya.Shetab
{
    public class BankingConfigItem
    {
        public string Name { get; set; }
        public string TerminalId { get; set; }
        public string MerchantId { get; set; }
        public string MerchantKey { get; set; }
        public string GatewayUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string RedirectType { get; set; }
        public string ReturnType { get; set; }
    }
    public class BankingConfig: List<BankingConfigItem>
    {
    }
}

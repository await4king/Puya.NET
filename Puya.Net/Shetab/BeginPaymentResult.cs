using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Shetab
{
    public class BeginPaymentResult
    {
        public Dictionary<string, string> Args { get; set; }
        public string PaymentId { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
        public BeginPaymentResult()
        {
            Date = DateTime.Now;
            Args = new Dictionary<string, string>();
        }
    }
}

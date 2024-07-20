using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Shetab
{
    public class EndPaymentResult
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
        public EndPaymentResult()
        {
            Date = DateTime.Now;
        }
    }
}

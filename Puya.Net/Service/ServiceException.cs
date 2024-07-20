using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puya.Service
{
    public class ServiceException: Exception
    {
        public string Status { get; set; }
        public ServiceException(string status, string msg): base(msg)
        {
            Status = status;
        }
        public ServiceException(string status, string msg, Exception inner) : base(msg, inner)
        {
            Status = status;
        }
    }
}

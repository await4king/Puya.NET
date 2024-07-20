using System;
using System.Collections.Generic;
using System.Text;
using Puya.Service;

namespace Puya.Sms
{
    public class SendResponseData
    {
        public object Data { get; set; }
        public object Response { get; set; }
        public Exception Error { get; set; }
    }
    public class SendResponse: ServiceResponse<SendResponseData>
    {
        public SendResponse()
        {
            Data = new SendResponseData();
        }
        public SendResponse(string mobile, string message)
        {
            Data = new SendResponseData();
        }
    }
}

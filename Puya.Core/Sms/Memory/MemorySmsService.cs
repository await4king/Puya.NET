using System;
using System.Collections.Generic;
using Puya.Service;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Sms
{
    public class MemorySmsService : SmsServiceBase<MemorySmsServiceConfig>
    {
        public List<SendResponse> Logs = new List<SendResponse>();
        public MemorySmsService(MemorySmsServiceConfig config, ISmsLogger logger) : base(config, logger)
        { }
        public MemorySmsService(ISmsLogger logger) : base(logger)
        { }
        public MemorySmsService(MemorySmsServiceConfig config) : base(config)
        { }
        public MemorySmsService()
        { }
        protected override SendResponse SendInternal(string mobile, string message)
        {
            var result = new SendResponse();
            
            result.Succeeded();

            Logs.Add(result);

            return result;
        }

        protected override Task<SendResponse> SendAsyncInternal(string mobile, string message, CancellationToken cancellation)
        {
            var result = SendInternal(mobile, message);

            return Task.FromResult(result);
        }
    }
}

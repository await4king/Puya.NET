using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Sms
{
    public class ConsoleSmsService: SmsServiceBase<ConsoleSmsServiceConfig>
    {
        public ConsoleSmsService(ConsoleSmsServiceConfig config, ISmsLogger logger) : base(config, logger)
        { }
        public ConsoleSmsService(ISmsLogger logger) : base(logger)
        { }
        public ConsoleSmsService(ConsoleSmsServiceConfig config) : base(config)
        { }
        public ConsoleSmsService()
        { }

        protected override Task<SendResponse> SendAsyncInternal(string mobile, string message, CancellationToken cancellation)
        {
            Console.WriteLine($"mobile: {mobile}, message: {message}");

            return Task.FromResult(null as SendResponse);
        }

        protected override SendResponse SendInternal(string mobile, string message)
        {
            Console.WriteLine($"mobile: {mobile}, message: {message}");

            return null;
        }
    }
}

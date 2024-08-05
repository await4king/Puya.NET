using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Sms
{
    public class DebugSmsService: SmsServiceBase<DebugSmsServiceConfig>
    {
        public DebugSmsService(DebugSmsServiceConfig config, ISmsLogger logger) : base(config, logger)
        { }
        public DebugSmsService(ISmsLogger logger) : base(logger)
        { }
        public DebugSmsService(DebugSmsServiceConfig config) : base(config)
        { }
        public DebugSmsService()
        { }

        protected override Task<SendResponse> SendAsyncInternal(string mobile, string message, CancellationToken cancellation)
        {
            System.Diagnostics.Debug.WriteLine($"mobile: {mobile}, message: {message}");

            return Task.FromResult(null as SendResponse);
        }

        protected override SendResponse SendInternal(string mobile, string message)
        {
            System.Diagnostics.Debug.WriteLine($"mobile: {mobile}, message: {message}");

            return null;
        }
    }
}

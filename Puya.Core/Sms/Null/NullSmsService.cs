using System;
using System.Collections.Generic;
using Puya.Service;
using System.Threading;
using System.Threading.Tasks;
using Puya.Extensions;

namespace Puya.Sms
{
    public class NullSmsService : ISmsService
    {
        public SendResponse Send(string mobile, string message, string category = null)
        {
            var result = new SendResponse();

            result.Succeeded();

            return result;
        }

        public Task<SendResponse> SendAsync(string mobile, string message, string category, CancellationToken cancellation)
        {
            var result = new SendResponse();

            result.Succeeded();

            return Task.FromResult(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Puya.Service;

namespace Puya.Sms
{
    public class FileSmsService : SmsServiceBase<FileSmsServiceConfig>
    {
        public FileSmsService(FileSmsServiceConfig config, ISmsLogger logger) : base(config, logger)
        { }
        public FileSmsService(ISmsLogger logger) : base(logger)
        { }
        public FileSmsService(FileSmsServiceConfig config) : base(config)
        { }
        public FileSmsService()
        { }
        string GetPath()
        {
            var filename = Config.FileName;
            var result = Config.Path;

            if (string.IsNullOrEmpty(filename))
            {
                filename = "sms.log";
            }

            if (string.IsNullOrEmpty(result))
            {
                result = AppDomain.CurrentDomain.BaseDirectory;

                var i = result.LastIndexOf("\\bin", StringComparison.CurrentCultureIgnoreCase);

                result = (i > 0 ? result.Substring(0, i) : result);
            }
            else
            {
                if (!Path.IsPathRooted(result))
                {
                    var path = AppDomain.CurrentDomain.BaseDirectory;

                    var i = path.LastIndexOf("\\bin", StringComparison.CurrentCultureIgnoreCase);

                    path = (i > 0 ? path.Substring(0, i) : path);

                    result = path + "\\" + result;
                }
            }

            result = Path.Combine(result, filename);

            return result;
        }
        protected override SendResponse SendInternal(string mobile, string message)
        {
            var path = GetPath();

            File.AppendAllText(path, $"{mobile}: {message}\n");

            return null;
        }

        protected override Task<SendResponse> SendAsyncInternal(string mobile, string message, CancellationToken cancellation)
        {
            var path = GetPath();

            File.AppendAllText(path, $"{mobile}: {message}\n");

            return Task.FromResult(null as SendResponse);
        }
    }
}

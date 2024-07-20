using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Puya.Extensions;

namespace Puya.Sms
{
    public class SmsLoggerConsole : ISmsLogger
    {
        public void Log(SmsLog log)
        {
            Console.WriteLine($"LogDate: {log.LogDate.ToString("yyyy/MM/dd HH:mm:ss.ffffff")}");

            if (!string.IsNullOrEmpty(log.Topic))
            {
                Console.WriteLine($"\tTopic: {log.Topic}");
            }
            if (!string.IsNullOrEmpty(log.MobileNo))
            {
                Console.WriteLine($"\tMobileNo: {log.MobileNo}");
            }
            if (!string.IsNullOrEmpty(log.Message))
            {
                Console.WriteLine($"\tMessage: {log.Message}");
            }
            if (log.Response != null)
            {
                Console.WriteLine($"\tResponse:\n{JsonConvert.SerializeObject(log.Response, Formatting.Indented)}");
            }
            if (log.Data != null)
            {
                Console.WriteLine($"\tData:\n{JsonConvert.SerializeObject(log.Data, Formatting.Indented)}");
            }
            if (log.Error != null)
            {
                Console.WriteLine($"\tError:\n{log.Error.ToString("\n")}");
            }
        }

        public Task LogAsync(SmsLog log, CancellationToken cancellation)
        {
            Log(log);

            return Task.CompletedTask;
        }
    }
}

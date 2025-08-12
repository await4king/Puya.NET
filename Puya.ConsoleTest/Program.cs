using Puya.Logging;
using System;

namespace Puya.ConsoleTest
{
    internal class Program
    {
        static void test_logger(ILogger logger)
        {
            logger.Info("hello");
            logger.Debug("BeginJob", "this is a message", () => new { a = 10, b = true, c = "test" });
            logger.Log(new Log
            {
                LogType = LogType.Info,
                AppId = 12,
                User = "user7637",
                BrowserName = "Edge",
                BrowserVersion = "12.0",
                Category = "Signing",
                ContentType = "application/json",
                Cookies = "",
                Data = new { id = 123, name = "ali", age = 34 },
                Form = "",
                Headers = "x-debug: 0, x-log: normal",
                Ip = "127.0.0.1",
                Message = "request data",
                OperationResult = OperationResult.Success,
                Method = "POST",
                Url = "https://www.mywebsite.com/api/user/10",
                Referrer = "https://www.google.com",
                MemberName = "test_console_logger",
                File = "C:\\projects\\Puya.ConsoleTest\\Program.cs",
                Line = 18,
                StackTrace = Environment.StackTrace
            });

            logger.Error("an unexpected situation happened. please check logs!", new { size = 300, code = "iusyhdiluy87214" });
        }
        static void Main(string[] args)
        {
            test_logger(new ConsoleLogger());
            test_logger(new DebugLogger());

            Console.ReadKey();
        }
    }
}

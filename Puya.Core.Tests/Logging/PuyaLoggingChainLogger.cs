using Newtonsoft.Json;
using Puya.Logging;

namespace Puya.Core.Tests.Logging
{
    public class MyLogger : ILogger
    {
        public void Clear()
        { }

        public Task ClearAsync(CancellationToken cancellation)
        { return Task.CompletedTask; }
        public void Log(Log log)
        {
            Console.WriteLine(log.Browser.Name);
            Console.WriteLine(JsonConvert.SerializeObject(log, Formatting.Indented));
        }

        public Task LogAsync(Log log, CancellationToken cancellation)
        {
            Log(log);

            return Task.CompletedTask;
        }
    }
    public class PuyaLoggingChainLogger
    {
        [Fact]
        public void Test_Log_1()
        {
            var lg1 = new MyLogger();
            var lg2 = new MemoryLogger();
            var logger = new ChainLogger(lg1, lg2);

            var log = new Log();

            log.Message = "hello";
            log.Browser = new Browser { Id = 1, Name = "MyBrowser" };

            Assert.True(logger.Loggers.Length > 0);
            Assert.True(logger.Loggers.Length == 3);
            Assert.True(logger.Loggers[logger.Loggers.Length - 1] as NullLogger != null);

            logger.Log(log);

            Assert.True(lg2.Logs.Count == 1);
        }
        [Fact]
        public void Test_Log_2()
        {
            var lg1 = new MyLogger();
            var lg2 = new MemoryLogger();
            var logger = new ChainLogger(lg1, lg2);

            var log = new Log();

            log.Message = "hello";

            logger.Log(log);

            Assert.True(lg2.Logs.Count == 2);
            Assert.NotNull(lg2.Logs[0].StackTrace);
            Assert.Null(lg2.Logs[1].StackTrace);
        }
    }
}

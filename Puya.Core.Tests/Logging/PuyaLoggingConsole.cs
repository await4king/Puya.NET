using Puya.Logging;

namespace Puya.Core.Tests.Logging
{
    public class PuyaLoggingConsole
    {
        ConsoleLogger GetLogger()
        {
            return new ConsoleLogger();
        }
        [Fact]
        public void Test_Log_1()
        {
            var logger = GetLogger();

            logger.Info("hello");
            logger.Debug("BeginJob", "this is a message", () => new { a = 10, b = true, c = "test" });

            Assert.True(true);
        }
    }
}

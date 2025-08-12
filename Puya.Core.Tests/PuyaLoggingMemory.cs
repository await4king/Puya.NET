using Puya.Logging;

namespace Puya.Core.Tests
{
    public class PuyaLoggingMemory
    {
        MemoryLogger GetLogger()
        {
            return new MemoryLogger();
        }
        [Fact]
        public void Test_Log_1()
        {
            var logger = GetLogger();

            logger.Info("hello");

            Assert.Single(logger.Logs);
        }
    }
}

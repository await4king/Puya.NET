using Puya.Logging;

namespace Puya.Core.Tests
{
    public class PuyaLoggingJsonFile
    {
        [Fact]
        public void Test_Log_1()
        {
            var logger = new JsonFileLogger();

            logger.Info("hello");
            logger.Debug("BeginJob", "this is a message", () => new { a = 10, b = true, c = "test" });

            var logfile = Path.Combine(Environment.CurrentDirectory, "log.log");

            Assert.True(File.Exists(logfile));

            var logs = logger.LoadLogFile(logfile);

            File.Delete(logfile);
            
            Assert.True(logs.Count > 0);
        }
        [Fact]
        public void Test_Log_2()
        {
            var logger = new JsonFileLogger(new JsonFileLoggerConfig { MaxChunk = 5, MaxSize = 1024 });

            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");

            var date = DateTime.Now.ToString("yyyyMMdd");
            var existingLogFiles = Directory.GetFiles(Environment.CurrentDirectory, logger.Config.FileName + "-" + date + "-*" + logger.Config.FileExtension);

            foreach (var item in existingLogFiles)
            {
                File.Delete(item);
            }

            Assert.True(existingLogFiles.Length > 0);
            Assert.True(existingLogFiles.Length == 3);
        }
        [Fact]
        public void Test_Log_3()
        {
            var logger = new JsonFileLogger(new JsonFileLoggerConfig { MaxChunk = 5, MaxSize = 950 });

            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");

            var date = DateTime.Now.ToString("yyyyMMdd");
            var existingLogFiles = Directory.GetFiles(Environment.CurrentDirectory, logger.Config.FileName + "-" + date + "-*" + logger.Config.FileExtension);

            foreach (var item in existingLogFiles)
            {
                File.Delete(item);
            }

            Assert.True(existingLogFiles.Length > 0);
            Assert.True(existingLogFiles.Length == 5);
        }
    }
}

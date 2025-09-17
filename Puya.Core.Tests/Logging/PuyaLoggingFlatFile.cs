using Puya.Logging;

namespace Puya.Core.Tests.Logging
{
    public class PuyaLoggingFlatFile
    {
        [Fact]
        public void Test_Log_1()
        {
            var logger = new FileLogger(new FileLoggerConfig { });

            logger.Info("hello");
            logger.Debug("BeginJob", "this is a message", () => new { a = 10, b = true, c = "test" });

            var logfile = Path.Combine(Environment.CurrentDirectory, "log.txt");

            Assert.True(File.Exists(logfile));

            var hasContent = File.ReadAllLines(logfile).Length > 0;

            File.Delete(logfile);

            Assert.True(hasContent);
        }
        [Fact]
        public void Test_Log_2()
        {
            var config = new FileLoggerConfig { MaxChunk = 5, MaxSize = 1024 };
            var logger = new FileLogger(config);

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
            var existingLogFiles = Directory.GetFiles(Environment.CurrentDirectory, config.FileName + "-" + date + "-*" + config.FileExtension);

            foreach (var item in existingLogFiles)
            {
                File.Delete(item);
            }

            Assert.True(existingLogFiles.Length > 0);
            Assert.True(existingLogFiles.Length == 2);
        }
        [Fact]
        public void Test_Log_3()
        {
            var config = new FileLoggerConfig { MaxChunk = 3, MaxSize = 600 };
            var logger = new FileLogger(config);

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
            var existingLogFiles = Directory.GetFiles(Environment.CurrentDirectory, config.FileName + "-" + date + "-*" + config.FileExtension);

            foreach (var item in existingLogFiles)
            {
                File.Delete(item);
            }

            Assert.True(existingLogFiles.Length > 0);
            Assert.True(existingLogFiles.Length == 3);
        }
    }
}

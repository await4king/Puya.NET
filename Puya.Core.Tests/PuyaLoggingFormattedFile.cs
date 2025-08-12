using Puya.Logging;

namespace Puya.Core.Tests
{
    public class PuyaLoggingFormattedFile
    {
        [Fact]
        public void Test_Log_1()
        {
            var config = new FormattedFileLoggerConfig { };
            var logger = new FormattedFileLogger(config);

            logger.Info("hello");
            logger.Debug("BeginJob", "this is a message", () => new { a = 10, b = true, c = "test" });

            var logfile = Path.Combine(config.Path, config.FileName + config.FileExtension);

            Assert.True(File.Exists(logfile));

            var hasContent = File.ReadAllLines(logfile).Length > 0;

            File.Delete(logfile);

            Assert.True(hasContent);
        }
        [Fact]
        public void Test_Log_2()
        {
            var config = new FormattedFileLoggerConfig { MaxChunk = 5, MaxSize = 1024 };
            var logger = new FormattedFileLogger(config);

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
            var existingLogFiles = Directory.GetFiles(config.Path, config.FileName + "-" + date + "-*" + config.FileExtension);

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
            var config = new FormattedFileLoggerConfig { MaxChunk = 3, MaxSize = 600 };
            var logger = new FormattedFileLogger(config);

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
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");
            logger.Info("hello");

            var date = DateTime.Now.ToString("yyyyMMdd");
            var existingLogFiles = Directory.GetFiles(config.Path, config.FileName + "-" + date + "-*" + config.FileExtension);

            foreach (var item in existingLogFiles)
            {
                File.Delete(item);
            }

            Assert.True(existingLogFiles.Length > 0);
            Assert.True(existingLogFiles.Length == 3);
        }
        [Fact]
        public void Test_Log_LoadLogs()
        {
            var config = new FormattedFileLoggerConfig { };
            var logger = new FormattedFileLogger(config);

            logger.Info("hello");
            logger.Debug("BeginJob", "this is a message", () => new { a = 10, b = true, c = "test" });

            var logfile = Path.Combine(config.Path, config.FileName + config.FileExtension);

            Assert.True(File.Exists(logfile));

            var logs = logger.LoadLogFile(logfile);

            File.Delete(logfile);

            Assert.NotNull(logs);
            Assert.True(logs.Count == 2);
            Assert.True(logs[0].LogType == LogType.Info);
            Assert.True(logs[0].Message == "hello");
            Assert.True(logs[1].LogType == LogType.Debug);
            Assert.True(logs[1].Category == "BeginJob");
            Assert.True(logs[1].Message == "this is a message");
        }
    }
}

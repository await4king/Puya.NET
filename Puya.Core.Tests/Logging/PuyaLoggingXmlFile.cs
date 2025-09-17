using Puya.Logging;

namespace Puya.Core.Tests.Logging
{
    public class PuyaLoggingXmlFile
    {
        [Fact]
        public void Test_Log_1()
        {
            var logger = new XmlFileLogger();

            logger.Info("hello");
            logger.Debug("BeginJob", "this is a message", () => new { a = 10, b = true, c = "test" });

            var logfile = Path.Combine(Environment.CurrentDirectory, "log.xml");

            Assert.True(File.Exists(logfile));

            var logs = logger.LoadLogFile(logfile);

            File.Delete(logfile);
            
            Assert.True(logs.Count > 0);
            Assert.True(logs[0].LogType == LogType.Info);
            Assert.True(logs[0].Message == "hello");
            Assert.True(logs[1].LogType == LogType.Debug);
            Assert.True(logs[1].Category == "BeginJob");
            Assert.True(logs[1].Message == "this is a message");
            Assert.True(logs[1].Data != null);
        }
        [Fact]
        public void Test_Log_2()
        {
            var logger = new XmlFileLogger(new XmlFileLoggerConfig { MaxChunk = 5, MaxSize = 1024 });

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
        [Fact]
        public void Test_Log_3()
        {
            var logger = new XmlFileLogger(new XmlFileLoggerConfig { MaxChunk = 3, MaxSize = 512 });

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
    }
}

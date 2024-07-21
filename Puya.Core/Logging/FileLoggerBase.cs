using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Models;
using Puya.Conversion;
using System.Diagnostics;

namespace Puya.Logging
{
    public abstract class FileLoggerBaseConfigBase : BaseLoggerConfig
    {
        public string FileName { get; set; }
        public virtual string FileExtension { get; set; }
        public string Path { get; set; }
        public int MaxSize { get; set; }
        public int MaxChunk { get; set; }
        public bool Repeat { get; set; }
        #region ctor
        public FileLoggerBaseConfigBase() : this(null, null)
        { }
        public FileLoggerBaseConfigBase(ILogFormatter formatter) : this(formatter, null)
        { }
        public FileLoggerBaseConfigBase(ILogFormatter formatter, ILogConfigProvider logConfigProvider) : base(formatter, logConfigProvider)
        {
            FileName = "log";
            FileExtension = ".log";
            Path = Environment.CurrentDirectory;
            MaxSize = -1;
            MaxChunk = -1;
            Repeat = false;
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new StringLogFormatter();
        }
        #endregion
    }
    public abstract class FileLoggerBase<TConfig> : BaseLogger<TConfig>
        where TConfig: FileLoggerBaseConfigBase, IBaseLoggerConfig, new()
    {
        public FileLoggerBase() : this(null, null)
        { }
        public FileLoggerBase(FileLoggerBaseConfigBase config) : this(config, null)
        { }
        public FileLoggerBase(FileLoggerBaseConfigBase config, ILogger next) : base(config, next)
        {
        }
        protected virtual string GetDate()
        {
            return DateTime.Now.ToString("yyyyyMMdd");
        }
        protected virtual string FormatLogFileName(string date, string chunk)
        {
            return StrongConfig.FileName + (string.IsNullOrEmpty(chunk) ? "" : "-" + date + "-" + chunk) + StrongConfig.FileExtension;
        }
        protected virtual string GetChunkNo(string filename, string defaultChunk = "")
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            var index = name.LastIndexOf('-');

            return index >= 0 ? name.Substring(index + 1) : defaultChunk;
        }
        protected string GetLogFile(string data, out bool reset)
        {
            var basePath = Path.IsPathRooted(StrongConfig.Path) ? StrongConfig.Path : Environment.CurrentDirectory + "\\" + StrongConfig.Path;
            var path = "";
            var chunk = "";
            var date = "";
            reset = false;

            if (StrongConfig.MaxSize > 0)
            {
                date = GetDate();
                var existingLogFiles = Directory.GetFiles(basePath, FormatLogFileName(date, "*"));
                var firstMax = existingLogFiles
                    .Where(f =>
                    {
                        var fi = new FileInfo(f);

                        return fi.Length < StrongConfig.MaxSize;
                    }).Max(f => GetChunkNo(f));

                if (!string.IsNullOrEmpty(firstMax))
                {
                    chunk = firstMax;
                }
                else
                {
                    long size = 0;
                    var max = existingLogFiles.Max(f => GetChunkNo(f, "0"));
                    var maxChunk = SafeClrConvert.ToInt(max);
                    var lastLogFile = basePath + "\\" + FormatLogFileName(date, maxChunk.ToString());

                    if (File.Exists(lastLogFile))
                    {
                        var fi = new FileInfo(lastLogFile);

                        size = fi.Length;
                    }

                    if (size + (data?.Length ?? 0) > StrongConfig.MaxSize)
                    {
                        if (maxChunk < StrongConfig.MaxChunk - 1)
                        {
                            chunk = (maxChunk + 1).ToString();
                        }
                        else
                        {
                            if (StrongConfig.Repeat)
                            {
                                var nextChunk = existingLogFiles.OrderByDescending(f =>
                                {
                                    var fi = new FileInfo(f);

                                    return fi.LastWriteTime;
                                }).Take(1).Select(f => GetChunkNo(f, "0")).FirstOrDefault();

                                reset = true;

                                var nextChunkNo = string.IsNullOrEmpty(nextChunk) ? 0 : SafeClrConvert.ToInt(nextChunk);

                                if (nextChunkNo < StrongConfig.MaxChunk - 1)
                                {
                                    nextChunkNo++;
                                }
                                else
                                {
                                    nextChunkNo = 0;
                                }

                                Debug.WriteLine("Repeat Logging on chunk {0}", nextChunkNo);

                                chunk = nextChunkNo.ToString();
                            }
                            else
                            {
                                chunk = maxChunk.ToString();
                            }
                        }
                    }
                    else
                    {
                        chunk = maxChunk.ToString();
                    }
                }
            }

            path = basePath + "\\" + FormatLogFileName(date, chunk);

            return path;
        }
        protected override void LogInternal(Log log)
        {
            bool reset;
            var data = Config.Formatter.Format(log);
            var path = GetLogFile(data, out reset);

            if (reset)
            {
                Write(path, data);
            }
            else
            {
                Append(path, data);
            }
        }
        protected virtual void Write(string path, string data)
        {
            File.WriteAllText(path, data);
        }
        protected virtual void Append(string path, string data)
        {
            File.AppendAllText(path, data);
        }
        public override void Clear()
        {
            bool reset;
            var path = GetLogFile("", out reset);

            File.WriteAllText(path, "");
        }
        public abstract List<Log> LoadLogFile(string path);
    }
}

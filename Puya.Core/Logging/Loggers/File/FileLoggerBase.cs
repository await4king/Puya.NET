using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Puya.Conversion;
using System.Diagnostics;

namespace Puya.Logging
{
    public abstract class FileLoggerBase<TConfig> : BaseLogger<TConfig>
        where TConfig : FileLoggerConfigBase, new()
    {
        public FileLoggerBase() : this(null, null)
        { }
        public FileLoggerBase(TConfig config) : this(config, null)
        { }
        public FileLoggerBase(TConfig config, ILogger next) : base(config, next)
        {
        }
        protected virtual string GetDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        protected virtual string FormatLogFileName(string date, string chunk)
        {
            return Config.FileName + (string.IsNullOrEmpty(chunk) ? "" : "-" + date + "-" + chunk.ToString().PadLeft(Config.MaxChunk.ToString().Length, '0')) + Config.FileExtension;
        }
        protected virtual string GetChunkNo(string filename, string defaultChunk = "")
        {
            var name = Path.GetFileNameWithoutExtension(filename);
            var index = name.LastIndexOf('-');

            return index >= 0 ? name.Substring(index + 1) : defaultChunk;
        }
        protected string GetLogFile(string data, out bool reset)
        {
            var basePath = Path.IsPathRooted(Config.Path) ? Config.Path : Environment.CurrentDirectory + "\\" + Config.Path;
            var path = "";
            var chunk = "";
            var date = "";

            reset = false;

            if (Config.MaxSize > 0)
            {
                date = GetDate();

                var existingLogFiles = Directory.GetFiles(basePath, FormatLogFileName(date, "*"));
                var firstMax = existingLogFiles
                    .Where(f =>
                    {
                        var fi = new FileInfo(f);

                        return fi.Length + data?.Length < Config.MaxSize;
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

                    if (size + (data?.Length ?? 0) > Config.MaxSize)
                    {
                        if (maxChunk < Config.MaxChunk - 1)
                        {
                            chunk = (maxChunk + 1).ToString();
                        }
                        else
                        {
                            if (Config.Repeat)
                            {
                                var nextChunk = existingLogFiles.OrderByDescending(f =>
                                {
                                    var fi = new FileInfo(f);

                                    return fi.LastWriteTime;
                                }).Take(1).Select(f => GetChunkNo(f, "0")).FirstOrDefault();

                                reset = true;

                                var nextChunkNo = string.IsNullOrEmpty(nextChunk) ? 0 : SafeClrConvert.ToInt(nextChunk);

                                if (nextChunkNo < Config.MaxChunk - 1)
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

            if (!reset && (!File.Exists(path) || new FileInfo(path).Length == 0))
            {
                reset = true;
            }

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

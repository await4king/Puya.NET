using System;
using System.Collections.Generic;
using System.IO;
using Puya.Conversion;

namespace Puya.Logging
{
    public class CsvFileLogger : FileLoggerBase<CsvFileLoggerConfig>
    {
        public CsvFileLogger() : this(null, null)
        { }
        public CsvFileLogger(CsvFileLoggerConfig config) : this(config, null)
        { }
        public CsvFileLogger(CsvFileLoggerConfig config, ILogger next) : base(config, next)
        { }
        private string GetHeader()
        {
            var cols = this.Config.Formatter.LogItems.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var header = "";

            foreach (var col in cols)
            {
                header += col + Config.ColSeparator;
            }

            return header + Config.RowSeparator;
        }
        protected override void Write(string path, string data)
        {
            var header = GetHeader();

            data = header + data;

            base.Write(path, data);
        }
        protected override void Append(string path, string data)
        {
            if (!File.Exists(path) || new FileInfo(path).Length == 0)
            {
                var header = GetHeader();

                data = header + data;
            }

            base.Append(path, data);
        }
        public override List<Log> LoadLogFile(string path)
        {
            var result = new List<Log>();

            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                var lines = content.Split(Config.RowSeparator);
                var header = null as string[];
                var first = 0;

                foreach (var line in lines)
                {
                    if (first++ == 0)
                    {
                        header = line.Split(Config.ColSeparator);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(line) && line != Config.RowSeparator.ToString())
                        {
                            var formatter = Config.Formatter as BaseLogFormatter;
                            var items = formatter.Decode(line)?.Split(Config.ColSeparator);

                            if (items != null && items.Length > 0)
                            {
                                var log = new Log();

                                for (var i = 0; i < items.Length; i++)
                                {
                                    var value = items[i];

                                    if (i < header.Length && !string.IsNullOrEmpty(value))
                                    {
                                        switch (header[i].ToLower())
                                        {
                                            case "id":
                                                log.Id = SafeClrConvert.ToInt(value);
                                                break;
                                            case "appid":
                                                log.AppId = SafeClrConvert.ToInt(value);
                                                break;
                                            case "operationresult":
                                                OperationResult r;

                                                if (Enum.TryParse(value, out r))
                                                {
                                                    log.OperationResult = r;
                                                }
                                                break;
                                            case "result":
                                                log.Result = SafeClrConvert.ToByte(value);
                                                break;
                                            case "category":
                                                log.Category = value;
                                                break;
                                            case "file":
                                                log.File = value;
                                                break;
                                            case "line":
                                                log.Line = SafeClrConvert.ToInt(value);
                                                break;
                                            case "membername":
                                                log.MemberName = value;
                                                break;
                                            case "message":
                                                log.Message = value;
                                                break;
                                            case "stacktrace":
                                                log.StackTrace = value;
                                                break;
                                            case "ip":
                                                log.Ip = value;
                                                break;
                                            case "user":
                                                log.User = value;
                                                break;
                                            case "logdate":
                                                log.LogDate = DateTime.Parse(value);
                                                break;
                                            case "data":
                                                log.Data = formatter.DataConverter.Deserialize(value);
                                                break;
                                        }
                                    }
                                }

                                result.Add(log);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}

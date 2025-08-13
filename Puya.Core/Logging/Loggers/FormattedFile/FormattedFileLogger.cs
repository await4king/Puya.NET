using Puya.Conversion;
using Puya.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puya.Logging
{
    public class FormattedFileLogger : FileLoggerBase<FormattedFileLoggerConfig>
    {
        public FormattedFileLogger() : this(null, null)
        { }
        public FormattedFileLogger(FormattedFileLoggerConfig config) : this(config, null)
        { }
        public FormattedFileLogger(FormattedFileLoggerConfig config, ILogger next) : base(config, next)
        { }
        private string GetHeader()
        {
            if (this.Config.DetailedFormatter == null)
            {
                throw new InvalidOperationException("specified log-formatter does not implement IDetailedLogFormatter interface.");
            }

            return this.Config.DetailedFormatter.LogParts
                                .Where(x => BaseLogFormatter.IsValidLogItem(x.Key))
                                .Select(x => x.Key)
                                .ToList()
                                .Join(Config.ColSeparator) + Config.RowSeparator;
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
                var cs = new FormattedFileSerializer(Config.ColSeparator, Config.RowSeparator);
                var rows = cs.DeserializeAll(content);
                var map = new List<string>();
                var i = 0;

                foreach (var row in rows)
                {
                    if (i++ == 0)
                    {
                        foreach (var item in row)
                        {
                            map.Add(item);
                        }
                    }
                    else
                    {
                        var log = new Log();

                        for (var j = 0; j < row.Count; j++)
                        {
                            var item = row[j];

                            switch (map[j])
                            {
                                case "id": log.Id = SafeClrConvert.ToInt(item); break;
                                case "appid": log.AppId = SafeClrConvert.ToInt(item); break;
                                case "threadid": log.ThreadId = string.IsNullOrWhiteSpace(item) ? null: (int?)SafeClrConvert.ToInt(item); break;
                                case "operationresult":
                                    if (Enum.TryParse(item, out OperationResult or))
                                    {
                                        log.OperationResult = or;
                                    }

                                    break;
                                case "category": log.Category = item; break;
                                case "file": log.File = item; break;
                                case "line": log.Line = SafeClrConvert.ToInt(item); break;
                                case "membername": log.MemberName = item; break;
                                case "message": log.Message = item; break;
                                case "stacktrace": log.StackTrace = item; break;
                                case "ip": log.Ip = item; break;
                                case "user": log.User = item; break;
                                case "logdate": log.LogDate = DateTime.Parse(item); break;
                                case "logtype":
                                    if (Enum.TryParse(item, out LogType lt))
                                    {
                                        log.LogType = lt;
                                    }

                                    break;
                                case "data": log.Data = item; break;
                                case "method": log.Method = item; break;
                                case "url": log.Url = item; break;
                                case "browsername": log.BrowserName = item; break;
                                case "browserversion": log.BrowserVersion = item; break;
                                case "referrer": log.Referrer = item; break;
                                case "headers": log.Headers = item; break;
                                case "form": log.Form = item; break;
                                case "cookies": log.Cookies = item; break;
                                case "body": log.Body = item; break;
                                case "contentType": log.ContentType = item; break;
                            }
                        }

                        result.Add(log);
                    }
                }
            }

            return result;
        }
    }
}

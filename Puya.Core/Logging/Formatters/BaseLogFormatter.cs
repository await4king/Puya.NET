using Puya.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Puya.Logging
{
    public abstract class BaseLogFormatter : IDetailedLogFormatter
    {
        public virtual bool IncludeNullValues { get; set; }
        public virtual bool EncodeData { get; set; }
        public static readonly string[] DefaultLogItems;
        static BaseLogFormatter()
        {
            DefaultLogItems = new string[]
            {
                "id",
                "appid",
                "threadid",
                "operationresult",
                "category",
                "file",
                "line",
                "membername",
                "message",
                "stacktrace",
                "ip",
                "user",
                "logdate",
                "logtype",
                "data",
                "method",
                "url",
                "browsername",
                "browserversion",
                "referrer",
                "headers",
                "form",
                "cookies",
                "body",
                "contentType"
            };
        }
        public BaseLogFormatter(string logItems) : this(null, logItems)
        { }
        public BaseLogFormatter(ILogDataConverter converter, string logItems)
        {
            LogItems = logItems?.ToLower();

            if (string.IsNullOrEmpty(logItems) || logItems == "*")
            {
                LogItems = GetDefaultLogItems();
            }
        }
        protected virtual ILogDataConverter GetDefaultDataConverter()
        {
            return null;
        }
        private ILogDataConverter _dataConverter;
        public virtual ILogDataConverter DataConverter
        {
            get
            {
                if (_dataConverter == null)
                {
                    _dataConverter = GetDefaultDataConverter();
                }
                if (_dataConverter == null)
                {
                    _dataConverter = new JsonLogDataConverter();
                }

                return _dataConverter;
            }
            set { _dataConverter = value; }
        }
        Dictionary<string, string> logParts;
        public Dictionary<string, string> LogParts
        {
            get { return logParts; }
            set
            {
                logParts = value;
                LogItems = logParts.Where(x => IsValidLogItem(x.Key)).Select(x => x.Key).Join(",");
            }
        }
        string logItems;
        public string LogItems
        {
            get { return logItems; }
            set
            {
                logItems = ValidateLogItems(value);
            }
        }
        public static bool IsValidLogItem(string logItem)
        {
            return DefaultLogItems.Contains(logItem);
        }
        protected static string ValidateLogItems(string items)
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(items))
            {
                foreach (var item in items.Split(',', MyStringSplitOptions.TrimToLowerAndRemoveEmptyEntries))
                {
                    if (IsValidLogItem(item))
                    {
                        result.Add(item);
                    }
                }
            }

            return result.Join(",");
        }
        protected virtual string FormatDate(DateTime date)
        {
            return date.ToString("yyyy/MM/dd HH:mm:ss.fffffff");
        }
        protected virtual string GetDefaultLogItems()
        {
            return DefaultLogItems.Join(",");
        }
        protected virtual string GetPartSeparator()
        {
            return "";
        }
        protected virtual string GetLogSeparator()
        {
            return "";
        }
        protected virtual string GetPropValue(Log log, string propName)
        {
            var result = string.Empty;

            switch (propName?.ToLower())
            {
                case "id": result = log.Id > 0 ? log.Id.ToString() : ""; break;
                case "appid": result = log.AppId.HasValue && log.AppId.Value > 0 ? log.AppId.Value.ToString() : ""; break;
                case "threadid": result = log.ThreadId.HasValue && log.ThreadId.Value > 0 ? log.ThreadId.Value.ToString() : ""; break;
                case "type": result = log.Type.ToString(); break;
                case "logtype": result = log.LogType.ToString(); break;
                case "result": result = log.Result.ToString(); break;
                case "operationresult": result = log.OperationResult.ToString(); break;
                case "category": result = log.Category; break;
                case "file": result = log.File; break;
                case "line": result = log.Line.ToString(); break;
                case "membername": result = log.MemberName; break;
                case "message": result = log.Message; break;
                case "stacktrace": result = log.StackTrace; break;
                case "ip": result = log.Ip; break;
                case "user": result = log.User; break;
                case "logdate": result = FormatDate(log.LogDate); break;
                case "data": result = this.SerializeData(log); break;
                case "method": result = log.Method; break;
                case "url": result = log.Url; break;
                case "browsername": result = log.BrowserName; break;
                case "browserversion": result = log.BrowserVersion; break;
                case "referrer": result = log.Referrer; break;
                case "headers": result = log.Headers; break;
                case "form": result = log.Form; break;
                case "cookies": result = log.Cookies; break;
                case "body": result = log.Body; break;
                case "contenttype": result = log.ContentType; break;
            }

            return result;
        }
        protected virtual void OnFormatPart(Log log, string part, string value, string format, string formattedValue)
        { }
        protected virtual void OnBeginFormat(Log log)
        { }
        protected virtual void OnEndFormat(Log log)
        { }
        protected bool Equals(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }
        public string Format(string template, string name, string value)
        {
            return template.Replace("{" + name.ToLower() + "}", value);
        }
        protected virtual string FormatInternal(Log log)
        {
            var result = "";

            var _logItems = string.IsNullOrEmpty(LogItems) || LogItems == "*" ? GetDefaultLogItems() : LogItems;

            if (LogParts != null && LogParts.Count > 0 && !string.IsNullOrEmpty(_logItems))
            {
                var logItems = _logItems.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var partSeparator = GetPartSeparator();
                var logSeparator = GetLogSeparator();
                var parts = LogParts.Where(x => !string.IsNullOrEmpty(x.Key) && !string.IsNullOrEmpty(x.Value)).ToList();

                OnBeginFormat(log);

                for (var i = 0; i < parts.Count; i++)
                {
                    var part = parts[i];

                    if (part.Key.StartsWith("raw", StringComparison.OrdinalIgnoreCase))
                    {
                        OnFormatPart(log, null, null, part.Value, part.Value);

                        result += part.Value + (i < parts.Count - 1 ? partSeparator: "");
                        continue;
                    }

                    if (part.Key.StartsWith("mixed", StringComparison.OrdinalIgnoreCase))
                    {
                        var formattedValue = part.Value;
                        var oldFormattedValue = formattedValue;

                        foreach (var item in logItems)
                        {
                            if (string.IsNullOrEmpty(item) || formattedValue.IndexOf("{" + item.ToLower() + "}") < 0)
                            {
                                continue;
                            }

                            var propValue = GetPropValue(log, item);

                            if (!string.IsNullOrEmpty(propValue) || IncludeNullValues)
                            {
                                formattedValue = Format(formattedValue, item, propValue);
                            }
                        }

                        if (!Equals(oldFormattedValue, formattedValue))
                        {
                            formattedValue += partSeparator;

                            OnFormatPart(log, part.Key, part.Value, "", formattedValue);

                            result += formattedValue;
                        }

                        continue;
                    }
                    else
                    {
                        var item = logItems.FirstOrDefault(x => Equals(x, part.Key));

                        if (string.IsNullOrEmpty(item))
                        {
                            continue;
                        }

                        var propValue = GetPropValue(log, item);

                        if (!string.IsNullOrEmpty(propValue) || IncludeNullValues)
                        {
                            var formattedValue = Format(part.Value, item, propValue) + (i < parts.Count - 1 ? partSeparator : "");

                            OnFormatPart(log, item.ToLower(), propValue, part.Value, formattedValue);

                            result += formattedValue;
                        }
                    }
                }

                OnEndFormat(log);

                result += logSeparator;
            }

            return result;
        }
        public virtual string Format(Log log)
        {
            var result = "";

            if (log != null)
            {
                result = FormatInternal(log);
            }

            return result;
        }
    }
}

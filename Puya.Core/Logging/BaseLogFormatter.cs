using System;
using System.Collections.Generic;
using System.Linq;
using Puya.Base;
using Puya.Logging.Models;

namespace Puya.Logging
{
    public abstract class BaseLogFormatter: ILogFormatter
    {
        public bool IncludeNullValues { get; set; }
        public BaseLogFormatter(string logItems): this(null, logItems)
        { }
        public BaseLogFormatter(ILogDataConverter converter, string logItems)
        {
            LogItems = logItems?.ToLower();

            if (string.IsNullOrEmpty(logItems) || logItems == "*")
            {
                LogItems = "id,appid,operationresult,category,file,line,membername,message,stacktrace,ip,user,logdate,data";
            }

            if (converter == null)
            {
                DataConverter = GetDefaultDataConverter();
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
                return TypeHelper.EnsureInitialized<ILogDataConverter, JsonLogDataConverter>(ref _dataConverter);
            }
            set { _dataConverter = value; }
        }
        public Dictionary<string, string> LogParts { get; set; }
        public string LogItems { get; set; }
        protected virtual string FormatDate(DateTime date)
        {
            return date.ToString("yyyy/MM/dd HH:mm:ss.fffffff");
        }
        protected virtual string SerializeData(object obj, string data)
        {
            var result = string.Empty;

            if (DataConverter != null)
                result = DataConverter.Serialize(obj);

            if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(data))
                result = Encode(data);

            return result;
        }
        public virtual string Encode(string x)
        {
            return x;
        }
        public virtual string Decode(string x)
        {
            return x;
        }
        protected virtual string GetPartSeparator()
        {
            return "";
        }
        protected virtual string GetLogSeparator()
        {
            return "";
        }
        protected virtual string FormatInternal(Log log)
        {
            var result = "";

            if (LogParts != null && LogParts.Count > 0)
            {
                var data = SerializeData(log.DataObject, log.Data);
                var logItems = LogItems.Split(new char[] { ',' });
                var partSeparator = GetPartSeparator();
                var logSeparator = GetLogSeparator();

                foreach (var item in logItems)
                {
                    var part = LogParts.FirstOrDefault(x => string.Compare(x.Key, item, true) == 0);

                    if (!string.IsNullOrEmpty(part.Key))
                    {
                        switch (part.Key)
                        {
                            case "appid":
                                if ((log.AppId != null && log.AppId > 0) || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{appid}", log.AppId.ToString()) + partSeparator;
                                }
                                break;
                            case "id":
                                if (log.Id > 0 || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{id}", log.Id.ToString()) + partSeparator;
                                }
                                break;
                            case "result":
                                if (log.Result > 0 || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{result}", log.Result.ToString()) + partSeparator;
                                }
                                break;
                            case "operationresult":
                                if (log.OperationResult != OperationResult.Normal || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{operationresult}", log.OperationResult.ToString()) + partSeparator;
                                }

                                break;
                            case "type":
                                result += part.Value.Replace("{type}", log.Type.ToString()) + partSeparator;

                                break;
                            case "logtype":
                                result += part.Value.Replace("{logtype}", log.LogType.ToString()) + partSeparator;

                                break;
                            case "category":
                                if (!string.IsNullOrEmpty(log.Category) || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{category}", Encode(log.Category)) + partSeparator;
                                }
                                break;
                            case "file":
                                if (!string.IsNullOrEmpty(log.File) || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{file}", Encode(log.File)) + partSeparator;
                                }
                                break;
                            case "line":
                                if ((log.Line != null && log.Line > 0) || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{line}", log.Line.ToString()) + partSeparator;
                                }
                                break;
                            case "membername":
                                if (!string.IsNullOrEmpty(log.MemberName) || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{membername}", Encode(log.MemberName)) + partSeparator;
                                }
                                break;
                            case "message":
                                if (!string.IsNullOrEmpty(log.Message) || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{message}", Encode(log.Message)) + partSeparator;
                                }
                                break;
                            case "stacktrace":
                                if (!string.IsNullOrEmpty(log.StackTrace) || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{stacktrace}", Encode(log.StackTrace)) + partSeparator;
                                }
                                break;
                            case "user":
                                if (!string.IsNullOrEmpty(log.User) || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{user}", Encode(log.User)) + partSeparator;
                                }
                                break;
                            case "ip":
                                if (!string.IsNullOrEmpty(log.Ip) || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{ip}", log.Ip) + partSeparator;
                                }
                                break;
                            case "logdate":
                                result += part.Value.Replace("{logdate}", FormatDate(log.LogDate)) + partSeparator;

                                break;
                            case "data":
                                if ((!string.IsNullOrEmpty(data) && data != "null") || IncludeNullValues)
                                {
                                    result += part.Value.Replace("{data}", data) + partSeparator;
                                }
                                break;
                            default:
                                result += part.Value + partSeparator;

                                break;
                        }
                    }
                }

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

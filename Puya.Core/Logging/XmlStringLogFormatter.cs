using System;
using System.Collections.Generic;
using System.Text;
using Puya.Logging.Models;
using System.Net;
using Puya.Extensions;

namespace Puya.Logging
{
    public class XmlStringLogFormatter : BaseLogFormatter
    {
        public XmlStringLogFormatter() : this(null, null)
        { }
        public XmlStringLogFormatter(ILogDataConverter converter) : this(converter, null)
        { }
        public XmlStringLogFormatter(string format) : this(null, format)
        { }
        public XmlStringLogFormatter(ILogDataConverter converter, string logItems) : base(converter, logItems)
        {
            LogParts = new Dictionary<string, string>
            {
                ["logdate"] = "\t<LogDate>{logdate}</LogDate>\n",
                ["id"] = "\t<Id>{id}</Id>\n",
                ["appid"] = "\t<App>{appid}</App>\n",
                ["user"] = "\t<User>{user}</User>\n",
                ["ip"] = "\t<Ip>{ip}</Ip>",
                ["category"] = "\t<Category>{category}</Category>\n",
                ["result"] = "\t<Result>{operationresult}</Result>\n",
                ["operationresult"] = "\t<OperationResult>{operationresult}</OperationResult>\n",
                ["membername"] = "\t<MemberName>{membername}</MemberName>\n",
                ["type"] = "\t<Type>{type}</Type>\n",
                ["logtype"] = "\t<LogType>{logtype}</LogType>\n",
                ["file"] = "\t<File>{file}</File>\n",
                ["line"] = "\t<Line>{line}</Line>\n",
                ["message"] = "\t<Message>{message}</Message>\n",
                ["data"] = "\t<Data>{data}</Data>\n",
                ["stacktrace"] = "\t<StackTrace>{stacktrace}</StackTrace>\n",
            };
        }
        protected override ILogDataConverter GetDefaultDataConverter()
        {
            return new XmlDataConverter();
        }
        protected override string SerializeData(object obj, string data)
        {
            var result = string.Empty;

            if (DataConverter != null)
            {
                try
                {
                    result = DataConverter.Serialize(obj);

                    if (!DataConverter.GetType().DescendsFrom<XmlDataConverter>())
                    {
                        result = Encode(result);
                    }
                }
                catch
                {
                    result = new JsonLogDataConverter().Serialize(obj);

                    result = Encode(result);
                }
            }

            if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(data))
                result = Encode(data);

            return result;
        }
        public override string Encode(string x)
        {
            return WebUtility.HtmlEncode(x);
        }
        public override string Decode(string x)
        {
            return WebUtility.HtmlDecode(x);
        }
        protected override string FormatInternal(Log log)
        {
            return "<Log>\n" + base.FormatInternal(log) + "</Log>";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Puya.Base;
using Puya.Logging.Models;

namespace Puya.Logging
{
    public class StringLogFormatter : BaseLogFormatter
    {
        public StringLogFormatter() : this(null, null)
        { }
        public StringLogFormatter(ILogDataConverter converter) : this(converter, null)
        { }
        public StringLogFormatter(string format) : this(null, format)
        { }
        public StringLogFormatter(ILogDataConverter converter, string logItems): base(converter, logItems)
        {
            LogParts = new Dictionary<string, string>
            {
                ["logdate"] = "{logdate}\n",
                ["id"] = "{id}.\n",
                ["appid"] = "App: {appid}\n",
                ["user"] = "User: {user}\n",
                ["ip"] = "Ip: {ip}\n",
                ["category"] = "Category: {category}\n",
                ["operationresult"] = "Result: {operationresult}\n",
                ["membername"] = "MemberName: {membername}\n",
                ["file"] = "File: {file}\n",
                ["line"] = "Line: {line}\n",
                ["message"] = "\n{message}\n",
                ["data"] = "\nData:\n{data}\n",
                ["stacktrace"] = "\n{stacktrace}\n"
            };
        }
        protected override string GetLogSeparator()
        {
            return new string('-', 100) + "\n";
        }
    }
}

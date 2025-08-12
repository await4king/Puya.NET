using System.Collections.Generic;

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
            LogItems = "*";
            LogParts = new Dictionary<string, string>
            {
                ["logtype"] = "{logtype}",
                ["logdate"] = "{logdate}",
                ["id"] = "{id}.",
                ["appid"] = "App: {appid}",
                ["threadid"] = "ThreadId: {threadid}",
                ["user"] = "User: {user}",
                ["ip"] = "Ip: {ip}",
                ["category"] = "Category: {category}",
                ["operationresult"] = "Result: {operationresult}",
                ["membername"] = "MemberName: {membername}",
                ["mixed0"] = "File: {file}, line: {line}",
                ["message"] = "{message}",
                ["data"] = "Data:\n{data}",
                ["stacktrace"] = "StackTrace: {stacktrace}",
                ["method"] = "Http Method: {method}",
                ["url"] = "Url: {url}",
                ["mixed1"] = "Browser: {browsername} {browserversion}",
                ["referrer"] = "Referer: {referrer}",
                ["headers"] = "Request Headers: {headers}",
                ["form"] = "Request Form: {form}",
                ["cookies"] = "Request Cookies: {cookies}",
                ["body"] = "Request Body: {body}",
                ["contenttype"] = "Content-Type: {contenttype}",
            };
        }
        protected override string GetPartSeparator()
        {
            return "\n";
        }
        protected override string GetLogSeparator()
        {
            return new string('-', 100) + "\n";
        }
    }
}

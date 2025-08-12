using System;
using System.Collections.Generic;

namespace Puya.Logging
{
    public class FormattedFileStringLogFormatter : BaseLogFormatter
    {
        FormattedFileSerializer serializer;
        public string RowSeparator { get; set; }
        public char ColSeparator { get; set; }
        public override bool IncludeNullValues
        {
            get { return true; }
            set { }
        }
        public FormattedFileStringLogFormatter() : this(null, null, null)
        { }
        public FormattedFileStringLogFormatter(ILogDataConverter converter) : this(converter, null, null)
        { }
        public FormattedFileStringLogFormatter(ILogDataConverter converter, string logItems, char? colSeparator = null, string rowSeparator = "") : base(converter, logItems)
        {
            ColSeparator = colSeparator ?? ',';
            RowSeparator = string.IsNullOrEmpty(rowSeparator) ? Environment.NewLine: rowSeparator;
            serializer = new FormattedFileSerializer(ColSeparator, RowSeparator);

            IncludeNullValues = true;

            LogParts = new Dictionary<string, string>
            {
                ["logtype"] = "{logtype}",
                ["logdate"] = "{logdate}",
                ["id"] = "{id}",
                ["threadid"] = "{threadid}",
                ["appid"] = "{appid}",
                ["user"] = "{user}",
                ["ip"] = "{ip}",
                ["category"] = "{category}",
                ["operationresult"] = "{operationresult}",
                ["membername"] = "{membername}",
                ["file"] = "{file}",
                ["line"] = "{line}",
                ["message"] = "{message}",
                ["data"] = "{data}",
                ["stacktrace"] = "{stacktrace}",
            };
        }
        protected override string GetLogSeparator()
        {
            return RowSeparator;
        }
        protected override string GetPartSeparator()
        {
            return ColSeparator.ToString();
        }
        protected override string GetPropValue(Log log, string propName)
        {
            var result = base.GetPropValue(log, propName);

            serializer.ColSeparator = ColSeparator;
            serializer.RowSeparator = RowSeparator;

            result = serializer.Serialize(result);

            return result;
        }
        protected override ILogDataConverter GetDefaultDataConverter()
        {
            return new JsonLogDataConverter();
        }
    }
}

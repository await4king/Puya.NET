using System;
using System.Collections.Generic;
using System.Text;
using Puya.Collections;
using Puya.Logging.Models;

namespace Puya.Logging
{
    internal enum CsvEncodeStates
    {
        Start,
        Slash
    }
    public class CsvStringLogFormatter : BaseLogFormatter
    {
        private CsvSerializer csvSerializer;

        private char rowSeparator;
        public char RowSeparator
        {
            get { return rowSeparator; }
            set
            {
                rowSeparator = csvSerializer.RowSeparator = value;
            }
        }
        private char colSeparator;
        public char ColSeparator
        {
            get { return colSeparator; }
            set
            {
                colSeparator = csvSerializer.ColSeparator = value;
            }
        }
        public CsvStringLogFormatter() : this(null, null)
        { }
        public CsvStringLogFormatter(ILogDataConverter converter) : this(converter, null)
        { }
        public CsvStringLogFormatter(string format) : this(null, format)
        { }
        public CsvStringLogFormatter(ILogDataConverter converter, string logItems) : base(converter, logItems)
        {
            csvSerializer = new CsvSerializer();

            RowSeparator = '\n';
            ColSeparator = ';';

            IncludeNullValues = true;

            LogParts = new Dictionary<string, string>
            {
                ["logdate"] = "{logdate}",
                ["id"] = "{id}",
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
                ["stacktrace"] = "{stacktrace}"
            };
        }
        public override string Decode(string x)
        {
            return csvSerializer.Deserialize(x);
        }
        public override string Encode(string x)
        {
            return csvSerializer.Serialize(x);
        }
        protected override string FormatInternal(Log log)
        {
            return base.FormatInternal(log) + RowSeparator;
        }
        protected override ILogDataConverter GetDefaultDataConverter()
        {
            return new CsvLogDataConverter();
        }
        protected override string GetPartSeparator()
        {
            return ColSeparator.ToString();
        }
    }
}

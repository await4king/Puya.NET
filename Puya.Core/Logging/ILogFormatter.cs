using System.Collections.Generic;

namespace Puya.Logging
{
    public interface ILogFormatter
    {
        string Format(Log log);
    }
    public interface IDetailedLogFormatter: ILogFormatter
    {
        Dictionary<string, string> LogParts { get; set; }
        string LogItems { get; set; }
        ILogDataConverter DataConverter { get; set; }
    }
}

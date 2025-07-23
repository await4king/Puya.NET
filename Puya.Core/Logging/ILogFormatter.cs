using System.Collections.Generic;

namespace Puya.Logging
{
    public interface ILogFormatter
    {
        Dictionary<string, string> LogParts { get; set; }
        string LogItems { get; set; }
        ILogDataConverter DataConverter { get; set; }
        string Format(Log log);
    }
}

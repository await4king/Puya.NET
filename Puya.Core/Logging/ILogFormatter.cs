using Puya.Logging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Puya.Logging
{
    [XmlRoot("Logs")]
    public class LogListWrapper
    {
        [XmlElement("Log")]
        public List<LogWrapper> Items { get; set; }
    }
}

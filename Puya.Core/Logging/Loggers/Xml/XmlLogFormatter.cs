using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Puya.Logging
{
    public class XmlLogFormatter : ILogFormatter
    {
        public string Format(Log log)
        {
            var wrapper = new LogWrapper(log);
            var rootAttr = new XmlRootAttribute("Log");
            var serializer = new XmlSerializer(typeof(LogWrapper), rootAttr);
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };

            var emptyNamespaces = new XmlSerializerNamespaces();
            emptyNamespaces.Add("", "");

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                serializer.Serialize(xmlWriter, wrapper, emptyNamespaces);

                return stringWriter.ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Puya.Logging
{
    public class XmlDataConverter : ILogDataConverter
    {
        public object Deserialize(string data)
        {
            var result = null as object;

            if (!string.IsNullOrEmpty(data))
            {
                var serializer = new XmlSerializer(data.GetType());

                using (var reader = new StringReader(data))
                {
                    try
                    {
                        result = serializer.Deserialize(reader);
                    }
                    catch
                    { }
                }
            }

            return result;
        }
        public string Serialize(object data)
        {
            var result = "";

            if (data != null)
            {
                var serializer = new XmlSerializer(data.GetType());

                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, data, new XmlSerializerNamespaces() { });
                    result = writer.ToString();
                    var index = result.IndexOf("?>");
                    result = index >= 0 ? result.Substring(index + 2) : result;
                }
            }

            return result;
        }
    }
}

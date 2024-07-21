using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public class JsonLogDataConverter : ILogDataConverter
    {
        public bool ThrowConversionErrors { get; set; }
        public object Deserialize(string data)
        {
            object result = null;

            try
            {
                result = JsonConvert.DeserializeObject(data);
            }
            catch (Exception)
            {
                if (ThrowConversionErrors)
                    throw;
            }

            return result;
        }

        public string Serialize(object data)
        {
            var result = string.Empty;

            if (data != null)
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    result = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
                }
                catch (Exception)
                {
                    if (ThrowConversionErrors)
                        throw;
                }
            }

            return result;
        }
    }
}

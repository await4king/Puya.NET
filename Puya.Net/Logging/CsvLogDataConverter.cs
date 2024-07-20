using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Logging
{
    public class CsvLogDataConverter : ILogDataConverter
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
        public CsvLogDataConverter()
        {
            csvSerializer = new CsvSerializer();
        }
        public object Deserialize(string data)
        {
            var result = null as object;
            var csv = csvSerializer.Deserialize(data);

            try
            {
                result = JsonConvert.DeserializeObject(csv);
            }
            catch
            { }

            return result;
        }

        public string Serialize(object data)
        {
            var result = JsonConvert.SerializeObject(data);

            return csvSerializer.Serialize(result);
        }
    }
}

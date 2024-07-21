using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public interface ILogDataConverter
    {
        object Deserialize(string data);
        string Serialize(object data);
    }
}

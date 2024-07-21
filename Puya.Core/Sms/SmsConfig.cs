using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Puya.Sms
{
    public abstract class SmsConfigItem
    {
        public bool Debug { get; set; }
        public bool Active { get; set; }
        public abstract string Type { get; }
        public virtual Dictionary<string, string> ToDictionary()
        {
            var result = new Dictionary<string, string>();

            foreach (var prop in this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (prop.CanRead)
                {
                    result.Add(prop.Name, prop.GetValue(this)?.ToString());
                }
            }

            return result;
        }
    }
    public class SmsConfig: List<Dictionary<string, string>>
    {
    }
}

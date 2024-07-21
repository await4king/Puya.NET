using System.Collections.Generic;

namespace Puya.Sms
{
    public class AssemblySmsServiceConfig: SmsConfigItem
    {
        public string AssemblyName { get; set; }
        public string AssemblyPath { get; set; }
        public Dictionary<string, string> InnerConfig { get; set; }
        public override string Type { get { return "assembly"; } }
    }
}

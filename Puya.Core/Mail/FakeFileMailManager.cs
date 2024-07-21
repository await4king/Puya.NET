using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Mail
{
    public class FakeFileMailManager : FakeMailManager<FakeFileMailConfig>
    {
        string GetPath()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var i = path.LastIndexOf("\\bin", StringComparison.CurrentCultureIgnoreCase);

            path = (i > 0 ? path.Substring(0, i) : path) + "\\email.log";

            return path;
        }
        public FakeFileMailManager()
        {
            StrongConfig.Path = GetPath();
        }
        protected override void LogInternal(string data)
        {
            File.AppendAllText(StrongConfig.Path, data);
        }
    }
}

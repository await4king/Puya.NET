using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging.Web.Abstractions
{
    public interface IWebDbLoggerConfig: IDbLoggerConfig, IBaseWebLoggerConfig
    { }
    public abstract class WebDbLoggerConfig : BaseWebLoggerConfig, IWebDbLoggerConfig
    {
        public WebDbLoggerConfig() : this(null)
        { }
        public WebDbLoggerConfig(ILogFormatter formatter): base(formatter)
        { }
    }
}

using Puya.Logging.Web.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging.Web.Abstractions
{
    public interface IBaseWebLoggerConfig : IBaseLoggerConfig
    {
        void Prepare(WebLog log);
    }
    public abstract class BaseWebLoggerConfig: BaseLoggerConfig, IBaseWebLoggerConfig
    {
        public BaseWebLoggerConfig() : this(null)
        { }
        public BaseWebLoggerConfig(ILogFormatter formatter): base(formatter)
        { }
        public virtual void Prepare(WebLog log)
        { }
        public bool ThrowContextErrors { get; set; }
    }
}

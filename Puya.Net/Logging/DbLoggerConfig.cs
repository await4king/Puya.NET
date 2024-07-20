using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public interface IDbLoggerConfig: IBaseLoggerConfig
    { }
    public abstract class DbLoggerConfig : BaseLoggerConfig, IDbLoggerConfig
    {
        public DbLoggerConfig() : this(null, null)
        { }
        public DbLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public DbLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider) : base(formatter, logConfigProvider)
        { }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new StringLogFormatter();
        }
    }
}

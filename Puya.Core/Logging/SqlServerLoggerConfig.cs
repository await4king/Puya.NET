using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public interface ISqlServerLoggerConfig : IDbLoggerConfig
    { }
    public class SqlServerLoggerConfig: DbLoggerConfig, ISqlServerLoggerConfig
    {
        public SqlServerLoggerConfig() : this(null, null)
        { }
        public SqlServerLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public SqlServerLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider) : base(formatter, logConfigProvider)
        { }
    }
}

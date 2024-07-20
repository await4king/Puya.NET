using Puya.Data;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Services.LogManager
{
	public partial class TapLogManagerSql : TapLogManagerBase
    {
        public IDb Db { get; set; }
        partial void Init(TapLogManagerSqlConfig config, IDb db)
        {
            Db = db;
        }
    }
}

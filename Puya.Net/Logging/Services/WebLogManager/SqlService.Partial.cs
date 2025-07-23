using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Logging.Services.WebLogManager;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class PuyaWebLogManagerSql : PuyaWebLogManagerBase
    {
        public IDb Db { get; set; }
        partial void Init(PuyaWebLogManagerSqlConfig config, IDb db)
        {
            Db = db;
        }
    }
}

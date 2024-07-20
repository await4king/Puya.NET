using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class TapWebLogManagerSql : TapWebLogManagerBase
    {
        public IDb Db { get; set; }
        partial void Init(TapWebLogManagerSqlConfig config, IDb db)
        {
            Db = db;
        }
    }
}

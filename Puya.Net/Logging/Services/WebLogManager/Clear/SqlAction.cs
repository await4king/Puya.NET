using Puya.Logging.Services.WebLogManager.Clear;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class PuyaWebLogManagerSqlClearAction : PuyaWebLogManagerClearBaseAction
    {
        public PuyaWebLogManagerSqlClearAction(PuyaWebLogManagerSql owner) : base(owner)
        {
        }
	}
}

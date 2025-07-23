using Puya.Logging.Services.WebLogManager.DeleteByPK;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class PuyaWebLogManagerSqlDeleteByPKAction : PuyaWebLogManagerDeleteByPKBaseAction
    {
        public PuyaWebLogManagerSqlDeleteByPKAction(PuyaWebLogManagerSql owner) : base(owner)
        {
        }
	}
}

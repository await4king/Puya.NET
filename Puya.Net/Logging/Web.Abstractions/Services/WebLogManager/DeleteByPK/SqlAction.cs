using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class TapWebLogManagerSqlDeleteByPKAction : TapWebLogManagerDeleteByPKBaseAction
    {
        public TapWebLogManagerSqlDeleteByPKAction(TapWebLogManagerSql owner) : base(owner)
        {
        }
	}
}

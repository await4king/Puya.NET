using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Services.LogManager
{
	public partial class TapLogManagerSqlDeleteByPKAction : TapLogManagerDeleteByPKBaseAction
    {
        public TapLogManagerSqlDeleteByPKAction(TapLogManagerSql owner) : base(owner)
        {
        }
	}
}

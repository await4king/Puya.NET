using Puya.Data;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class TapWebLogManagerSqlClearAction : TapWebLogManagerClearBaseAction
    {
		private async Task DoRun(TapWebLogManagerClearRequest request, TapWebLogManagerClearResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as TapWebLogManagerSql;
			var query = "truncate table dbo.WebLogs";

			if (async)
			{
				await owner.Db.ExecuteNonQuerySqlAsync(query, null, cancellation);
			}
			else
			{
				owner.Db.ExecuteNonQuerySql(query);
			}

			response.Succeeded();
		}
		protected override void RunInternal(TapWebLogManagerClearRequest request, TapWebLogManagerClearResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapWebLogManagerClearRequest request, TapWebLogManagerClearResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

using Puya.Data;
using Puya.Extensions;
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
		private async Task DoRun(PuyaWebLogManagerClearRequest request, PuyaWebLogManagerClearResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as PuyaWebLogManagerSql;
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
		protected override void RunInternal(PuyaWebLogManagerClearRequest request, PuyaWebLogManagerClearResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(PuyaWebLogManagerClearRequest request, PuyaWebLogManagerClearResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

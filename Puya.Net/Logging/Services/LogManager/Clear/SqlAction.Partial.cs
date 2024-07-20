using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;

namespace Puya.Logging.Services.LogManager
{
	public partial class TapLogManagerSqlClearAction : TapLogManagerClearBaseAction
    {
		private async Task DoRun(TapLogManagerClearRequest request, TapLogManagerClearResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as TapLogManagerSql;
			var query = "truncate table dbo.Logs";

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
		protected override void RunInternal(TapLogManagerClearRequest request, TapLogManagerClearResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapLogManagerClearRequest request, TapLogManagerClearResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

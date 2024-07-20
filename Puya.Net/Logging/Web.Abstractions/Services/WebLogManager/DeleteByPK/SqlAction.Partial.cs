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
	public partial class TapWebLogManagerSqlDeleteByPKAction : TapWebLogManagerDeleteByPKBaseAction
    {
		private async Task DoRun(TapWebLogManagerDeleteByPKRequest request, TapWebLogManagerDeleteByPKResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as TapWebLogManagerSql;
			var query = $"delete from dbo.WebLogs where Id={request.Key}";

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
		protected override void RunInternal(TapWebLogManagerDeleteByPKRequest request, TapWebLogManagerDeleteByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapWebLogManagerDeleteByPKRequest request, TapWebLogManagerDeleteByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

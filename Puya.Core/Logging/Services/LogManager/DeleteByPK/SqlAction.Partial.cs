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
	public partial class TapLogManagerSqlDeleteByPKAction : TapLogManagerDeleteByPKBaseAction
    {
		private async Task DoRun(TapLogManagerDeleteByPKRequest request, TapLogManagerDeleteByPKResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as TapLogManagerSql;
			var query = $"delete from dbo.Logs where Id={request.Key}";

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
		protected override void RunInternal(TapLogManagerDeleteByPKRequest request, TapLogManagerDeleteByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapLogManagerDeleteByPKRequest request, TapLogManagerDeleteByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

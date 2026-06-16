using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Extensions;

namespace Puya.Logging.Services.LogManager
{
	public partial class PuyaLogManagerSqlDeleteByPKAction : PuyaLogManagerDeleteByPKBaseAction
    {
		private async Task DoRun(PuyaLogManagerDeleteByPKRequest request, PuyaLogManagerDeleteByPKResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as PuyaLogManagerSql;
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
		protected override void RunInternal(PuyaLogManagerDeleteByPKRequest request, PuyaLogManagerDeleteByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(PuyaLogManagerDeleteByPKRequest request, PuyaLogManagerDeleteByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

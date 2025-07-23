using Puya.Data;
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
		private async Task DoRun(PuyaWebLogManagerDeleteByPKRequest request, PuyaWebLogManagerDeleteByPKResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as PuyaWebLogManagerSql;
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
		protected override void RunInternal(PuyaWebLogManagerDeleteByPKRequest request, PuyaWebLogManagerDeleteByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(PuyaWebLogManagerDeleteByPKRequest request, PuyaWebLogManagerDeleteByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

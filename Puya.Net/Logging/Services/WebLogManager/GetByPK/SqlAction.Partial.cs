using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Logging;
using Puya.Logging.Services.WebLogManager.GetByPK;
using Puya.Extensions;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class PuyaWebLogManagerSqlGetByPKAction : PuyaWebLogManagerGetByPKBaseAction
    {
		private async Task DoRun(PuyaWebLogManagerGetByPKRequest request, PuyaWebLogManagerGetByPKResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as PuyaWebLogManagerSql;
			var query = $"select * from dbo.WebLogs where Id={request.Key}";

			if (async)
			{
				response.Data = await owner.Db.ExecuteSingleSqlAsync<Log>(query, (object)null, cancellation);
			}
			else
			{
				response.Data = owner.Db.ExecuteSingleSql<Log>(query);
			}

			response.Succeeded();
		}
		protected override void RunInternal(PuyaWebLogManagerGetByPKRequest request, PuyaWebLogManagerGetByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(PuyaWebLogManagerGetByPKRequest request, PuyaWebLogManagerGetByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

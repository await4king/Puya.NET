using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Web.Abstractions.Models;
using Puya.Data;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class TapWebLogManagerSqlGetByPKAction : TapWebLogManagerGetByPKBaseAction
    {
		private async Task DoRun(TapWebLogManagerGetByPKRequest request, TapWebLogManagerGetByPKResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as TapWebLogManagerSql;
			var query = $"select * from dbo.WebLogs where Id={request.Key}";

			if (async)
			{
				response.Data = await owner.Db.ExecuteSingleSqlAsync<WebLog>(query, (object)null, cancellation);
			}
			else
			{
				response.Data = owner.Db.ExecuteSingleSql<WebLog>(query);
			}

			response.Succeeded();
		}
		protected override void RunInternal(TapWebLogManagerGetByPKRequest request, TapWebLogManagerGetByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapWebLogManagerGetByPKRequest request, TapWebLogManagerGetByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

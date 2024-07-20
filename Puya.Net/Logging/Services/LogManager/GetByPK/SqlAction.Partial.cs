using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Models;
using Puya.Data;

namespace Puya.Logging.Services.LogManager
{
	public partial class TapLogManagerSqlGetByPKAction : TapLogManagerGetByPKBaseAction
    {
		private async Task DoRun(TapLogManagerGetByPKRequest request, TapLogManagerGetByPKResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as TapLogManagerSql;
			var query = $"select * from dbo.Logs where Id={request.Key}";

			if (async)
			{
				response.Data = await owner.Db.ExecuteSingleSqlAsync<Models.Log>(query, (object)null, cancellation);
			}
			else
			{
				response.Data = owner.Db.ExecuteSingleSql<Models.Log>(query);
			}

			response.Succeeded();
		}
		protected override void RunInternal(TapLogManagerGetByPKRequest request, TapLogManagerGetByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapLogManagerGetByPKRequest request, TapLogManagerGetByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

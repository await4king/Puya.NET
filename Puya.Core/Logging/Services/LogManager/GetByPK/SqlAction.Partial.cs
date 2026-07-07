using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Extensions;

namespace Puya.Logging.Services.LogManager
{
	public partial class PuyaLogManagerSqlGetByPKAction : PuyaLogManagerGetByPKBaseAction
    {
		private async Task DoRun(PuyaLogManagerGetByPKRequest request, PuyaLogManagerGetByPKResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as PuyaLogManagerSql;
			var query = $"select * from dbo.Logs where Id={request.Key}";

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
		protected override void RunInternal(PuyaLogManagerGetByPKRequest request, PuyaLogManagerGetByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(PuyaLogManagerGetByPKRequest request, PuyaLogManagerGetByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

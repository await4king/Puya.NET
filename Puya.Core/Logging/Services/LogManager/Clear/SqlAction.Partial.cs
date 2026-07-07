using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Extensions;

namespace Puya.Logging.Services.LogManager
{
	public partial class PuyaLogManagerSqlClearAction : PuyaLogManagerClearBaseAction
    {
		private async Task DoRun(PuyaLogManagerClearRequest request, PuyaLogManagerClearResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as PuyaLogManagerSql;
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
		protected override void RunInternal(PuyaLogManagerClearRequest request, PuyaLogManagerClearResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(PuyaLogManagerClearRequest request, PuyaLogManagerClearResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

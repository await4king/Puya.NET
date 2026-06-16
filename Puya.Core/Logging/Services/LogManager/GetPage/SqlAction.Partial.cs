using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Conversion;
using Puya.Extensions;

namespace Puya.Logging.Services.LogManager
{
	public partial class PuyaLogManagerSqlGetPageAction : PuyaLogManagerGetPageBaseAction
    {
		private async Task DoRun(PuyaLogManagerGetPageRequest request, PuyaLogManagerGetPageResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as PuyaLogManagerSql;

			if (async)
			{
				response.Data.Items = await owner.Db.ExecuteReaderCommandAsync<Log>("usp1_Logs_get_page", request, cancellation);
			}
			else
			{
				response.Data.Items = owner.Db.ExecuteReaderCommand<Log>("usp1_Logs_get_page");
			}

			response.Data.RecordCount = SafeClrConvert.ToInt(request.RecordCount.Value);
			response.Data.PageCount = SafeClrConvert.ToInt(request.PageCount.Value);

			response.Succeeded();
		}
		protected override void RunInternal(PuyaLogManagerGetPageRequest request, PuyaLogManagerGetPageResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(PuyaLogManagerGetPageRequest request, PuyaLogManagerGetPageResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

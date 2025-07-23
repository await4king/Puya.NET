using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Conversion;
using Puya.Logging;
using Puya.Logging.Services.WebLogManager.GetPage;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class PuyaWebLogManagerSqlGetPageAction : PuyaWebLogManagerGetPageBaseAction
    {
		private async Task DoRun(PuyaWebLogManagerGetPageRequest request, PuyaWebLogManagerGetPageResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as PuyaWebLogManagerSql;

			if (async)
			{
				response.Data.Items = await owner.Db.ExecuteReaderCommandAsync<WebLog>("usp1_WebLogs_get_page", request, cancellation);
			}
			else
			{
				response.Data.Items = owner.Db.ExecuteReaderCommand<WebLog>("usp1_WebLogs_get_page");
			}

			response.Data.RecordCount = SafeClrConvert.ToInt(request.RecordCount.Value);
			response.Data.PageCount = SafeClrConvert.ToInt(request.PageCount.Value);

			response.Succeeded();
		}
		protected override void RunInternal(PuyaWebLogManagerGetPageRequest request, PuyaWebLogManagerGetPageResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(PuyaWebLogManagerGetPageRequest request, PuyaWebLogManagerGetPageResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

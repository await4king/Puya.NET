using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Web.Abstractions.Models;
using Puya.Data;
using Puya.Conversion;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class TapWebLogManagerSqlGetPageAction : TapWebLogManagerGetPageBaseAction
    {
		private async Task DoRun(TapWebLogManagerGetPageRequest request, TapWebLogManagerGetPageResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as TapWebLogManagerSql;

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
		protected override void RunInternal(TapWebLogManagerGetPageRequest request, TapWebLogManagerGetPageResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapWebLogManagerGetPageRequest request, TapWebLogManagerGetPageResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

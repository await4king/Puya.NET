using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Models;
using Puya.Data;
using Puya.Conversion;

namespace Puya.Logging.Services.LogManager
{
	public partial class TapLogManagerSqlGetPageAction : TapLogManagerGetPageBaseAction
    {
		private async Task DoRun(TapLogManagerGetPageRequest request, TapLogManagerGetPageResponse response, bool async, CancellationToken cancellation)
		{
			var owner = Owner as TapLogManagerSql;

			if (async)
			{
				response.Data.Items = await owner.Db.ExecuteReaderCommandAsync<Models.Log>("usp1_Logs_get_page", request, cancellation);
			}
			else
			{
				response.Data.Items = owner.Db.ExecuteReaderCommand<Models.Log>("usp1_Logs_get_page");
			}

			response.Data.RecordCount = SafeClrConvert.ToInt(request.RecordCount.Value);
			response.Data.PageCount = SafeClrConvert.ToInt(request.PageCount.Value);

			response.Succeeded();
		}
		protected override void RunInternal(TapLogManagerGetPageRequest request, TapLogManagerGetPageResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapLogManagerGetPageRequest request, TapLogManagerGetPageResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

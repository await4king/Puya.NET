using Puya.Caching;
using Puya.Data;
using Puya.Debugging;
using Puya.Extensions;
using Puya.Logging;
using Puya.Service;
using Puya.ServiceModel;
using Puya.Settings;
using Puya.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Samples.Services.Products
{
	public partial class TapProductsServiceSqlServerGetAllDefaultAction : TapProductsServiceGetAllBaseAction
    {
		private async Task DoRun(TapProductsServiceGetAllRequest request, TapProductsServiceGetAllResponse response, bool async, CancellationToken cancellation)
		{
            response.Data = await Owner.Db.ExecuteReaderSqlDynamicAsync("select * from Products", null, async, cancellation);

            response.Succeeded();
        }
		protected override void RunInternal(TapProductsServiceGetAllRequest request, TapProductsServiceGetAllResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapProductsServiceGetAllRequest request, TapProductsServiceGetAllResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

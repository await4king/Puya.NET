using Puya.Collections;
using Puya.Logging;
using Puya.Data;
using Puya.Caching;
using Puya.Service;
using Puya.ServiceModel;
using Puya.Settings;
using Puya.Translation;
using Puya.Debugging;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Puya.Samples.Products.Products
{
	public partial class ProductServiceSqlServerGetAllDefaultAction : ProductServiceGetAllBaseAction
    {
		private async Task DoRun(ProductServiceGetAllRequest request, ProductServiceGetAllResponse response, bool async, CancellationToken cancellation)
		{
			response.Data = await Owner.Db.ExecuteReaderSqlDynamicAsync("select * from Products", null, async, cancellation);
			response.Succeeded();
		}
		protected override void RunInternal(ProductServiceGetAllRequest request, ProductServiceGetAllResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(ProductServiceGetAllRequest request, ProductServiceGetAllResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

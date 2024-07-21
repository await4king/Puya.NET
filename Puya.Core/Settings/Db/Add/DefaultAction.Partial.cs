using Puya.Service;
using Puya.Settings.Service.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Conversion;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsDefaultAddAction : TapDbSettingsAddBaseAction
    {
		private async Task DoRun(TapDbSettingsAddRequest request, TapDbSettingsAddResponse response, bool async, CancellationToken cancellation)
		{
            if (async)
            {
                await Db.ExecuteNonQueryCommandAsync("usp1_Settings_add", request, cancellation);
            }
            else
            {
                Db.ExecuteNonQueryCommand("usp1_Settings_add", request);
            }

            var result = SafeClrConvert.ToString(request.Result.Value);

            response.SetStatus(result);
            response.Message = SafeClrConvert.ToString(request.Message.Value);

            if (response.IsSucceeded())
            {
                Cache.Set(request.Key, request.Value, 0);
            }
        }
		protected override void RunInternal(TapDbSettingsAddRequest request, TapDbSettingsAddResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapDbSettingsAddRequest request, TapDbSettingsAddResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

using Puya.Service;
using Puya.ServiceModel;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Puya.Settings.Service.Models;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsDefaultDeleteByPKAction : TapDbSettingsDeleteByPKBaseAction
    {
		private async Task DoRun(TapDbSettingsDeleteByPKRequest request, TapDbSettingsDeleteByPKResponse response, bool async, CancellationToken cancellation)
		{
			await Task.CompletedTask;
		}
		protected override void RunInternal(TapDbSettingsDeleteByPKRequest request, TapDbSettingsDeleteByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapDbSettingsDeleteByPKRequest request, TapDbSettingsDeleteByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

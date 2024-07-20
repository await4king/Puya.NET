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
	public partial class TapDbSettingsDefaultGetPageAction : TapDbSettingsGetPageBaseAction
    {
		private async Task DoRun(TapDbSettingsGetPageRequest request, TapDbSettingsGetPageResponse response, bool async, CancellationToken cancellation)
		{
			await Task.CompletedTask;
		}
		protected override void RunInternal(TapDbSettingsGetPageRequest request, TapDbSettingsGetPageResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapDbSettingsGetPageRequest request, TapDbSettingsGetPageResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

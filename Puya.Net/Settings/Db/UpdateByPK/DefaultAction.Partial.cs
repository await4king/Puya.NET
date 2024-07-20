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
	public partial class TapDbSettingsDefaultUpdateByPKAction : TapDbSettingsUpdateByPKBaseAction
    {
		private async Task DoRun(TapDbSettingsUpdateByPKRequest request, TapDbSettingsUpdateByPKResponse response, bool async, CancellationToken cancellation)
		{
			await Task.CompletedTask;
		}
		protected override void RunInternal(TapDbSettingsUpdateByPKRequest request, TapDbSettingsUpdateByPKResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapDbSettingsUpdateByPKRequest request, TapDbSettingsUpdateByPKResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

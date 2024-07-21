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
	public partial class TapDbSettingsDefaultUpdateByKeyAction : TapDbSettingsUpdateByKeyBaseAction
    {
		private async Task DoRun(TapDbSettingsUpdateByKeyRequest request, TapDbSettingsUpdateByKeyResponse response, bool async, CancellationToken cancellation)
		{
			await Task.CompletedTask;
		}
		protected override void RunInternal(TapDbSettingsUpdateByKeyRequest request, TapDbSettingsUpdateByKeyResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapDbSettingsUpdateByKeyRequest request, TapDbSettingsUpdateByKeyResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

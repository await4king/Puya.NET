using System.Threading;
using System.Threading.Tasks;

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

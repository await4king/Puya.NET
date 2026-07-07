using System.Threading;
using System.Threading.Tasks;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsDefaultClearAllAction : TapDbSettingsClearAllBaseAction
    {
		private async Task DoRun(TapDbSettingsClearAllRequest request, TapDbSettingsClearAllResponse response, bool async, CancellationToken cancellation)
		{
			await Task.CompletedTask;
		}
		protected override void RunInternal(TapDbSettingsClearAllRequest request, TapDbSettingsClearAllResponse response)
		{
			DoRun(request, response, false, CancellationToken.None).Wait();
		}
        protected override async Task RunInternalAsync(TapDbSettingsClearAllRequest request, TapDbSettingsClearAllResponse response, CancellationToken cancellation)
        {
			await DoRun(request, response, true, cancellation);
		}
	}
}

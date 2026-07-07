using Puya.ServiceModel;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsGetPageBaseAction:
        TapBaseServiceAction<TapDbSettingsBase, TapDbSettingsGetPageRequest, TapDbSettingsGetPageResponse>
    {
        public TapDbSettingsGetPageBaseAction(TapDbSettingsBase owner) : base(owner)
        {
        }
    }
}

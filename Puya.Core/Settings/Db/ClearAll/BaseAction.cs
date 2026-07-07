using Puya.ServiceModel;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsClearAllBaseAction:
        TapBaseServiceAction<TapDbSettingsBase, TapDbSettingsClearAllRequest, TapDbSettingsClearAllResponse>
    {
        public TapDbSettingsClearAllBaseAction(TapDbSettingsBase owner) : base(owner)
        {
        }
    }
}

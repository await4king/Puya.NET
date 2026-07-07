using Puya.ServiceModel;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsGetByPKBaseAction:
        TapBaseServiceAction<TapDbSettingsBase, TapDbSettingsGetByPKRequest, TapDbSettingsGetByPKResponse>
    {
        public TapDbSettingsGetByPKBaseAction(TapDbSettingsBase owner) : base(owner)
        {
        }
    }
}

using Puya.ServiceModel;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsUpdateByPKBaseAction:
        TapBaseServiceAction<TapDbSettingsBase, TapDbSettingsUpdateByPKRequest, TapDbSettingsUpdateByPKResponse>
    {
        public TapDbSettingsUpdateByPKBaseAction(TapDbSettingsBase owner) : base(owner)
        {
        }
    }
}

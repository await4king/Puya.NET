using Puya.ServiceModel;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsUpdateByKeyBaseAction:
        TapBaseServiceAction<TapDbSettingsBase, TapDbSettingsUpdateByKeyRequest, TapDbSettingsUpdateByKeyResponse>
    {
        public TapDbSettingsUpdateByKeyBaseAction(TapDbSettingsBase owner) : base(owner)
        {
        }
    }
}

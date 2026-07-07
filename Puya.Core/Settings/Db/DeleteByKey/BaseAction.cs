using Puya.ServiceModel;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsDeleteByKeyBaseAction:
        TapBaseServiceAction<TapDbSettingsBase, TapDbSettingsDeleteByKeyRequest, TapDbSettingsDeleteByKeyResponse>
    {
        public TapDbSettingsDeleteByKeyBaseAction(TapDbSettingsBase owner) : base(owner)
        {
        }
    }
}

using Puya.ServiceModel;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsDeleteByPKBaseAction:
        TapBaseServiceAction<TapDbSettingsBase, TapDbSettingsDeleteByPKRequest, TapDbSettingsDeleteByPKResponse>
    {
        public TapDbSettingsDeleteByPKBaseAction(TapDbSettingsBase owner) : base(owner)
        {
        }
    }
}

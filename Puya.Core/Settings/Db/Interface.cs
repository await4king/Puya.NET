using Puya.Service;

namespace Puya.Settings.Service.Db
{
	public partial interface ITapDbSettings :IService
	{
        TapDbSettingsAddBaseAction Add { get; }
        TapDbSettingsUpdateByPKBaseAction UpdateByPK { get; }
        TapDbSettingsUpdateByKeyBaseAction UpdateByKey { get; }
        TapDbSettingsDeleteByPKBaseAction DeleteByPK { get; }
        TapDbSettingsClearAllBaseAction ClearAll { get; }
        TapDbSettingsDeleteByKeyBaseAction DeleteByKey { get; }
        TapDbSettingsGetPageBaseAction GetPage { get; }
        TapDbSettingsGetByPKBaseAction GetByPK { get; }
    }
}

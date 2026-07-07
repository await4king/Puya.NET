using Puya.Service;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsRegistration : ServiceRegistery
    {
        public TapDbSettingsRegistration()
        {
			Add(typeof(TapDbSettingsBase), typeof(TapDbSettingsDefault));
			Add(typeof(ITapDbSettings), typeof(TapDbSettingsDefault));
			Add(typeof(TapDbSettingsDefault), typeof(TapDbSettingsDefault));

            Add(typeof(TapDbSettingsAddBaseAction), typeof(TapDbSettingsDefaultAddAction));
            Add(typeof(TapDbSettingsUpdateByPKBaseAction), typeof(TapDbSettingsDefaultUpdateByPKAction));
            Add(typeof(TapDbSettingsUpdateByKeyBaseAction), typeof(TapDbSettingsDefaultUpdateByKeyAction));
            Add(typeof(TapDbSettingsDeleteByPKBaseAction), typeof(TapDbSettingsDefaultDeleteByPKAction));
            Add(typeof(TapDbSettingsClearAllBaseAction), typeof(TapDbSettingsDefaultClearAllAction));
            Add(typeof(TapDbSettingsDeleteByKeyBaseAction), typeof(TapDbSettingsDefaultDeleteByKeyAction));
            Add(typeof(TapDbSettingsGetPageBaseAction), typeof(TapDbSettingsDefaultGetPageAction));
            Add(typeof(TapDbSettingsGetByPKBaseAction), typeof(TapDbSettingsDefaultGetByPKAction));
		}
	}
}
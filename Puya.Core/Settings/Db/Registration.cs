using Puya.Service;
using Puya.ServiceModel;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Puya.Data;
using Puya.Logging;
using Puya.Caching;
using Puya.Settings;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsRegistration : ServiceRegistery
    {
        public TapDbSettingsRegistration()
        {
			Add(typeof(TapDbSettingsDefaultConfig), typeof(TapDbSettingsDefaultConfig));
			Add(typeof(TapDbSettingsBaseConfig), typeof(TapDbSettingsBaseConfig));
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